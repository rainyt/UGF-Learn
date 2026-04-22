using System;
using System.Collections.Generic;
using Data;
using FrameworkCore;
using Game;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    /// <summary>
    /// 基础子弹显示。
    /// </summary>
    public class BaseBullet : BaseDisplay
    {

        /// <summary>
        /// 子弹数据。
        /// </summary>
        public BulletData Data;

        /// <summary>
        /// 子弹速度。每秒移动的距离。
        /// </summary>
        public float Speed = 360;

        /// <summary>
        /// FPS。每秒更新的次数。
        /// </summary>
        public int Fps = 30;

        /// <summary>
        /// 子弹渲染器。
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// 子弹的渲染形象列表
        /// </summary>
        private List<Sprite> sprites;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            this.Data = (BulletData)userData;
            this.transform.position = Data.ParentHero.transform.position;
            this.spriteRenderer = GetComponent<SpriteRenderer>();

            var bulletData = GameData.tables.Tbbullets.Get(this.Data.Id);
            this.sprites = AssetsManager.Instance.GetSprites("Bullets:" + bulletData.Imageid);

            this.CurrentFrame = 0;
        }

        private int __currentFrame = 0;

        /// <summary>
        /// 当前帧。
        /// </summary>
        public int CurrentFrame
        {
            get => __currentFrame; set
            {
                if (sprites.Count > 0)
                {
                    var newFrame = value % sprites.Count;
                    if (__currentFrame != newFrame)
                    {
                        __currentFrame = newFrame;
                        this.spriteRenderer.sprite = this.sprites[__currentFrame];
                    }
                }
                else
                    this.spriteRenderer.sprite = null;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            AddVelocity(0, Speed * elapseSeconds);
            // 超出屏幕范围，销毁子弹
            var position = transform.position;
            if (position.x < Utils.System.LEFT_BOUNDARY || position.x > Utils.System.RIGHT_BOUNDARY
                || position.y > Utils.System.BOTTOM_BOUNDARY || position.y < Utils.System.TOP_BOUNDARY)
            {
                GameEntry.Entity.HideEntity(this.Entity.Id);
            }
            // 更新帧
            this.CurrentFrame = (int)Math.Round(this.NowTime / (1.0f / Fps));
        }
    }
}