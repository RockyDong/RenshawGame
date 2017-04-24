using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renshaw.MemoryModel
{
    public class PersonalModel<T> where T : IUserEntity
    {

        public void Load(int userId)
        {

        }

        public T GetOrAdd(int userId)
        {
            return default(T);
        }

        public bool Add(int userId, T value)
        {
            return true;
        }

        public bool AddOrUpdate(int userId, T value)
        {
            return true;
        }

        /// <summary>
        /// 以同步的方式更新Entity
        /// </summary>
        /// <param name="entityList"></param>
        public static bool SendSync(IEnumerable<AbstractEntity> entityList)
        {
            var keyList = new List<byte[]>();
            var valueList = new List<byte[]>();
            foreach (var entity in entityList)
            {
                if (entity == null)
                {
                    continue;
                }
                //watch post entity changed times.
//                ProfileManager.PostEntityOfMessageQueueTimes(entity.GetType().FullName, entity.GetKeyCode(), GetOperateMode(entity));

                entity.TempTimeModify = DateTime.Now;
                string key = string.Format("{0}_{1}|{2}",
                                            entity.GetType().FullName.Replace("_","%11"),
                                            entity.PersonalId,
                                            entity.GetKeyCode());
                var keyValues = key.Split('_', '|');
                string id_str = AbstractEntity.DecodeKeyCode(keyValues[1]);
                int id = int.Parse(id_str);
                string keyCode = keyValues[2];
                string redisKey = string.Format("{0}_{1}", keyValues[0], keyCode);
                byte[] idBytes = BufferUtils.GetBytes(id);
                var keyBytes = Encoding.UTF8.GetBytes(redisKey);
                bool isDelete = entity.IsDelete;
                byte[] entityBytes = ProtoBufUtils.Serialize(entity);
                //modify resean: set unchange status.
                entity.Reset();

                byte[] stateBytes = BufferUtils.GetBytes(isDelete ? 1 : 0);
                byte[] values = BufferUtils.MergeBytes(BufferUtils.GetBytes(idBytes.Length + stateBytes.Length), idBytes, stateBytes, entityBytes);
                keyList.Add(keyBytes);
                valueList.Add(values);
            }
            return ProcessRedisSyncQueue(string.Empty, keyList.ToArray(), valueList.ToArray());

        }

        public static string GetRedisEntityKeyName(string typeName)
        {
            typeName = typeName.Split('_')[0];
            string hashId = typeName.StartsWith("$")
                ? typeName
                : "$" + typeName;
            return hashId;
        }

        private static void DecodeValueBytes(byte[] buffer, out byte[] headBytes, out byte[] valBytes, out int state, out int identity)
        {
            var headLen = BitConverter.ToInt32(buffer, 0);
            headBytes = new byte[headLen + 4];
            valBytes = new byte[buffer.Length - headBytes.Length];
            Buffer.BlockCopy(buffer, 0, headBytes, 0, headBytes.Length);
            Buffer.BlockCopy(buffer, headBytes.Length, valBytes, 0, valBytes.Length);
            DecodeHeadBytes(headBytes, out identity, out state);
        }

        private static void DecodeHeadBytes(byte[] buffer, out int identity, out int state)
        {
            var idBytes = new byte[4];
            var stateBytes = new byte[4];
            int pos = 4;//Head's length(4)
            Buffer.BlockCopy(buffer, pos, idBytes, 0, idBytes.Length);
            pos += idBytes.Length;
            Buffer.BlockCopy(buffer, pos, stateBytes, 0, stateBytes.Length);
            pos += stateBytes.Length;

            identity = BitConverter.ToInt32(idBytes, 0);
            state = BitConverter.ToInt32(stateBytes, 0);
        }

        private static bool ProcessRedisSyncQueue(string sysnWorkingQueueKey, byte[][] keys, byte[][] values)
        {
            bool result = false;
            try
            {
                var redisSyncErrorQueue = new List<byte[][]>();
                var entityList = new List<RedisEntityItem>();
                var entityRemList = new List<RedisEntityItem>();
                var mutilKeyMapList = new List<RedisEntityItem>();
                var mutilKeyMapRemList = new List<RedisEntityItem>();
                var sqlWaitSyncQueue = new List<KeyValuePair<string, byte[][]>>();

                for (int i = 0; i < keys.Length; i++)
                {
                    byte[] keyBytes = keys[i];
                    byte[] valueBytes = values[i];
                    try
                    {
                        string[] queueKey = Encoding.UTF8.GetString(keyBytes).Split('_');
                        string entityTypeName = queueKey[0].TrimStart("$".ToCharArray()).Replace("%11", "_");
                        string entityParentKey = GetRedisEntityKeyName(queueKey[0]);
                        byte[] entityKeyBytes = Encoding.UTF8.GetBytes(queueKey[1]);
                        bool hasMutilKey = false;
                        bool isStoreInDb = true;
                        /*SchemaTable schema;
                        if (EntitySchemaSet.TryGet(entityTypeName, out schema))
                        {
                            hasMutilKey = RedisConnectionPool.CurrRedisInfo.ClientVersion >= RedisStorageVersion.HashMutilKeyMap &&
                                schema.EntityType.IsSubclassOf(typeof(BaseEntity)) &&
                                schema.Keys.Length > 1;
                            isStoreInDb = schema.StorageType.HasFlag(StorageType.WriteOnlyDB) || schema.StorageType.HasFlag(StorageType.ReadWriteDB);
                        }*/

                        byte[] headBytes;
                        byte[] entityValBytes;
                        int state;
                        int identity;
                        DecodeValueBytes(valueBytes, out headBytes, out entityValBytes, out state, out identity);

                        var entityItem = new RedisEntityItem()
                        {
                            HashId = entityParentKey,
                            UserId = identity,
                            KeyBytes = entityKeyBytes,
                            ValueBytes = entityValBytes,
                            State = state,
                            HasMutilKey = hasMutilKey
                        };
                        if (entityItem.State == 1)
                        {
                            entityRemList.Add(entityItem);
                            if (hasMutilKey) mutilKeyMapRemList.Add(entityItem);
                        }
                        else
                        {
                            entityList.Add(entityItem);
                            if (hasMutilKey) mutilKeyMapList.Add(entityItem);
                        }
                        /*if (_enableWriteToDb && isStoreInDb)
                        {
                            //增加到Sql等待队列
                            string sqlWaitQueueKey = GetSqlWaitSyncQueueKey(identity);
                            sqlWaitSyncQueue.Add(new KeyValuePair<string, byte[][]>(sqlWaitQueueKey, new[] { keyBytes, headBytes }));
                        }*/
                    }
                    catch (Exception error)
                    {
//                        TraceLog.WriteError("RedisSync key:{0} error:{1}", RedisConnectionPool.ToStringKey(keyBytes), error);
                        redisSyncErrorQueue.Add(new[] { keyBytes, valueBytes });
                    }
                }
                var redisErrorKeys = redisSyncErrorQueue.Select(p => p[0]).ToArray();
                var redisErrorValues = redisSyncErrorQueue.Select(p => p[1]).ToArray();
                var sqlWaitGroups = sqlWaitSyncQueue.GroupBy(p => p.Key);
                var setGroups = entityList.GroupBy(p => p.HashId);
                var removeGroups = entityRemList.GroupBy(p => p.HashId);
                var mutilKeyMapGroups = mutilKeyMapList.GroupBy(p => p.HashId);
                var mutilKeyMapRemGroups = mutilKeyMapRemList.GroupBy(p => p.HashId);

                /*RedisConnectionPool.ProcessPipeline(pipeline =>
                {
                    bool hasPost = false;

                    foreach (var g in setGroups)
                    {
                        string typeName = RedisConnectionPool.DecodeTypeName(g.Key);
                        var keyCodes = new List<string>();
                        var entityKeys = g.Select(p =>
                        {
                            keyCodes.Add(RedisConnectionPool.ToStringKey(p.KeyBytes));
                            return p.KeyBytes;
                        }).ToArray();
                        var entityValues = g.Select(p => p.ValueBytes).ToArray();
                        pipeline.QueueCommand(client => ((RedisClient)client).HMSet(g.Key, entityKeys, entityValues), () =>
                        {//onsuccess
                            ProfileManager.ProcessEntityOfMessageQueueTimes(typeName, keyCodes, OperateMode.Add | OperateMode.Modify);
                        });
                        hasPost = true;
                    }
                    foreach (var g in removeGroups)
                    {
                        string typeName = RedisConnectionPool.DecodeTypeName(g.Key);
                        var keyCodes = new List<string>();
                        var keybytes = g.Select(p =>
                        {
                            keyCodes.Add(RedisConnectionPool.ToStringKey(p.KeyBytes));
                            return p.KeyBytes;
                        }).ToArray();
                        pipeline.QueueCommand(client => ((RedisClient)client).HDel(g.Key, keybytes), () =>
                        {//onsuccess
                            ProfileManager.ProcessEntityOfMessageQueueTimes(typeName, keyCodes, OperateMode.Remove);
                        });
                        hasPost = true;
                    }
                    foreach (var g in mutilKeyMapGroups)
                    {
                        //create mutil-key index from storage.
                        string hashId = g.Key;
                        var subGroup = g.GroupBy(t => t.UserId);
                        foreach (var @group in subGroup)
                        {
                            string firstKey = AbstractEntity.EncodeKeyCode(@group.Key.ToString());
                            var keybytes = @group.Select(p => p.KeyBytes).ToArray();
                            pipeline.QueueCommand(client => RedisConnectionPool.SetMutilKeyMap((RedisClient)client, hashId, firstKey, keybytes));
                            hasPost = true;
                        }
                    }
                    foreach (var g in mutilKeyMapRemGroups)
                    {
                        //delete mutil-key index from storage.
                        string hashId = g.Key;
                        var subGroup = g.GroupBy(t => t.UserId);
                        foreach (var @group in subGroup)
                        {
                            string firstKey = AbstractEntity.EncodeKeyCode(@group.Key.ToString());
                            var keybytes = @group.Select(p => p.KeyBytes).ToArray();
                            pipeline.QueueCommand(client => RedisConnectionPool.RemoveMutilKeyMap((RedisClient)client, hashId, firstKey, keybytes));
                            hasPost = true;
                        }
                    }

                    if (redisErrorKeys.Length > 0)
                    {
                        pipeline.QueueCommand(client => ((RedisClient)client).HMSet(RedisSyncErrorQueueKey, redisErrorKeys, redisErrorValues));
                        hasPost = true;
                    }
                    foreach (var g in sqlWaitGroups)
                    {
                        var sqlWaitKeys = g.Select(p => p.Value[0]).ToArray();
                        var sqlWaitValues = g.Select(p => p.Value[1]).ToArray();
                        int count = sqlWaitKeys.Length;
                        pipeline.QueueCommand(client => ((RedisClient)client).HMSet(g.Key, sqlWaitKeys, sqlWaitValues), () =>
                        {//onsuccess
                            ProfileManager.WaitSyncSqlOfMessageQueueTimes(null, count);
                        });
                        hasPost = true;
                    }
                    if (hasPost)
                    {
                        pipeline.Flush();
                    }
                    
                    result = redisErrorKeys.Length == 0;
                });*/
            }
            catch (Exception ex)
            {
//                TraceLog.WriteError("DoProcessRedisSyncQueue error:{0}", ex);
//                try
//                {
//                    RedisConnectionPool.Process(client => client.HMSet(RedisSyncErrorQueueKey, keys, values));
//                }
//                catch (Exception er)
//                {
//                    TraceLog.WriteError("Put RedisSyncErrorQueue error:{0}", er);
//                }
            }
            return result;
        }
    }
}
