namespace Game
{
    using Data;
    using Displays;
    using GameFramework.Event;
    using GameFramework.Fsm;
    using GameFramework.Procedure;
    using UnityEngine;
    using UnityGameFramework.Runtime;
    using Utils;
    public class ProcedureGame : ProcedureBase
    {
        private AssetsManager assetsManager;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log($"ProcedureLaunch OnEnter, IL2CPP: {System.IsIL2CPP}");

            // 开始加载图片
            assetsManager = new AssetsManager();
            assetsManager.LoadFile("Assets/Images/Background/Background1.png");
            assetsManager.LoadFile("Assets/Displays/BaseImage.prefab");
            assetsManager.OnProgress((progress) =>
            {
                Debug.Log($"AssetsManager OnProgress: {progress}");
            });
            assetsManager.Start((success, message) =>
            {
                if (success)
                {
                    Debug.Log("Assets loaded successfully: " + message);
                    // 新增一张图片到舞台上
                    this.onStartGame();
                    // GameEntry.Entity.ShowEntity<Image>(1, "Assets/Displays/BaseImage.prefab", "Stage", assetsManager.GetTexture2D("loading"));
                    // GameEntry.Entity.ShowEntity<Image>(2, "Assets/Displays/BaseImage.prefab", "Stage", assetsManager.GetTexture2D("logo"));
                }
                else
                {
                    Debug.LogError("Failed to load assets: " + message);
                }
            });
        }

        private bool isGameStarted = false;

        public int HeroId = -1;

        private float createEnemyInterval = 1;

        private float lastCreateEnemyTime = 0;

        public void onStartGame()
        {
            System.UpdateScreen();


            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, onShowEntitySuccess);

            GameEntry.Entity.ShowEntity<Background>("Assets/Displays/BaseImage.prefab", "Background", new BackgroundData { Texture = assetsManager.GetTexture2D("Background1"), Speed = 30, Index = 0 });
            GameEntry.Entity.ShowEntity<Background>("Assets/Displays/BaseImage.prefab", "Background", new BackgroundData { Texture = assetsManager.GetTexture2D("Background1"), Speed = 30, Index = 1 });

            HeroId = GameEntry.Entity.ShowEntity<Hero>("Assets/Images/Hero.prefab", "Stage");

            isGameStarted = true;

        }

        private void onShowEntitySuccess(object sender, GameEventArgs e)
        {
            // Debug.Log($"onShowEntitySuccess: {e}");
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!isGameStarted)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Entity entity = GameEntry.Entity.GetEntity(HeroId);
                if (entity != null)
                {
                    Hero hero = entity.Logic as Hero;
                    if (hero != null)
                    {
                        hero.MoveTo(worldPos.x, worldPos.y);
                    }
                }
            }

            lastCreateEnemyTime += elapseSeconds;
            if (lastCreateEnemyTime >= createEnemyInterval)
            {
                lastCreateEnemyTime = 0;
                GameEntry.Entity.ShowEntity<Enemy>("Assets/Images/Enemys/Golem.prefab", "Enemys");
            }

            // 子弹之间的碰撞
            var bullets = GameEntry.Entity.GetEntityGroup("Bullets").GetAllEntities();
            var enemys = GameEntry.Entity.GetEntityGroup("Enemys").GetAllEntities();
            for (int i = bullets.Length - 1; i >= 0; i--)
            {
                Entity entity = bullets[i] as Entity;
                var bullet = entity.Logic as BaseDisplay;
                for (int j = 0; j < enemys.Length; j++)
                {
                    Entity enemyEntity = enemys[j] as Entity;
                    var enemy = enemyEntity.Logic as Enemy;
                    if (bullet.DistanceTo(enemy) < 0.16)
                    {
                        // 这里应该要产生击中效果
                        GameEntry.Entity.ShowEntity<HitEffect>("Assets/Images/BoomEffect.prefab", "Stage", new HitEffectData { X = entity.transform.position.x, Y = entity.transform.position.y, Scale = 1f });
                        enemy.Hurt(1);
                        GameEntry.Entity.HideEntity(entity.Id);
                        break;
                    }
                }
            }

        }
    }

}