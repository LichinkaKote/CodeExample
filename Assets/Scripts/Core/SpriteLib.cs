using RSG;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class SpriteLib
    {
        public Sprite[] UIicons { get; private set; }
        public Sprite[] Items { get; private set; }
        public Sprite[] Weapons { get; private set; }
        public Sprite[] Enemies { get; private set; }
        public Sprite[] MapObjects { get; private set; }
        public IPromise Load()
        {
            return Promise.All(
                LoadUIicons(),
                LoadItems(),
                LoadEmemy(),
                LoadWeapons(),
                LoadMapObjects()
                );
        }
        private Promise LoadUIicons()
        {
            var result = new Promise();
            AdressableLoader.LoadAssetAsync<Sprite[]>(Strings.UIicons).Then(data =>
            {
                UIicons = data;
                result.Resolve();
            })
                .Catch(result.Reject);
            return result;
        }
        private Promise LoadItems()
        {
            var result = new Promise();
            AdressableLoader.LoadAssetAsync<Sprite[]>(Strings.Items).Then(data =>
            {
                Items = data;
                result.Resolve();
            })
                .Catch(result.Reject);
            return result;
        }
        private Promise LoadWeapons()
        {
            var result = new Promise();
            AdressableLoader.LoadAssetAsync<Sprite[]>(Strings.Weapons).Then(data =>
            {
                Weapons = data;
                result.Resolve();
            })
                .Catch(result.Reject);
            return result;
        }
        private Promise LoadEmemy()
        {
            var result = new Promise();
            AdressableLoader.LoadAssetAsync<Sprite[]>(Strings.EnemyIcons).Then(data =>
            {
                Enemies = data;
                result.Resolve();
            })
                .Catch(result.Reject);
            return result;
        }
        private Promise LoadMapObjects()
        {
            var result = new Promise();
            AdressableLoader.LoadAssetAsync<Sprite[]>(Strings.MapObjects).Then(data =>
            {
                MapObjects = data;
                result.Resolve();
            })
                .Catch(result.Reject);
            return result;
        }
    }
}