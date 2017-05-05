using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace Renshaw.Commom
{
    public class PetService
    {
        private CacheCollection petCache;
        public PetService()
        {
            Init();
        }

        public void Load(int userId)
        {
            if (petCache[userId.ToString()] != null)
                return;
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                var pets = connection.Select<Pet>(m => m.UserId == userId);
                var petDict = new ConcurrentDictionary<int, object>();
                foreach (var pet in pets)
                {
                    petDict.TryAdd(pet.UniqueId, pet);
                }
                petCache.Add(userId.ToString(), petDict);
            }
        }

        public void Unload(int userId)
        {
            petCache.Remove(userId.ToString());
        }

        public void Init()
        {
            petCache = new CacheCollection(1000);
        }

        public Pet GetPet(int userId,int uniqueId)
        {
            var petDict = (ConcurrentDictionary<int, object>)petCache[userId.ToString()];
            var pet = petDict[uniqueId];
            return (Pet)pet;
        }

        public List<Pet> GetUserPets(int userId)
        {
            var petDict = (ConcurrentDictionary<int, object>)petCache[userId.ToString()];
            List<Pet> pets = new List<Pet>();
            foreach (var petKeyValuePair in petDict)
            {
                pets.Add((Pet)petKeyValuePair.Value);
            }
            return pets;
        }
    }
}
