using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    delegate void OnAnimationEnd();

    class Animation : Component
    {
        protected int numFrames;
        protected float frameDuration;
        protected bool isPlaying;
        protected int frameWidth;
        protected int frameHeight;
        protected int currentFrame;

        protected float elapsedTime;

        protected OnAnimationEnd animationEndCallback;

        public bool Loop;

        public bool IsPlaying { get { return isPlaying; } }

        public Animation(GameObject owner, int frameW, int frameH, int numFrames, float framePerSeconds, bool loop = true, OnAnimationEnd animEndCallback=null) : base(owner)
        {
            this.frameWidth = frameW;
            this.frameHeight = frameH;

            this.frameDuration = 1 / framePerSeconds;

            this.numFrames = numFrames;

            Loop = loop;

            animationEndCallback = animEndCallback;
        }

        public override void Update()
        {
            if (isPlaying)
            {
                elapsedTime += Game.DeltaTime;

                if (elapsedTime >= frameDuration)
                {
                    currentFrame++;
                    elapsedTime = 0;

                    if (currentFrame >= numFrames)
                    {
                        //animation end
                        if (Loop)
                        {
                            currentFrame = 0;
                        }
                        else
                        {
                            OnAnimationEnd();
                            return;
                        }
                    }

                    owner.TextureOffsetX = frameWidth * currentFrame;

                }
            }
        }

        protected virtual void OnAnimationEnd()
        {
            isPlaying = false;
            animationEndCallback?.Invoke();
        }

        public virtual void Play()
        {
            isPlaying = true;
        }

        public virtual void Stop()
        {
            isPlaying = false;
            currentFrame = 0;
            elapsedTime = 0;
        }

        public virtual void Pause()
        {
            isPlaying = false;
        }
    }
}
