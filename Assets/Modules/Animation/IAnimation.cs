using System.Collections;

namespace Common
{
    public interface IAnimation
    {
        void Play();
        void Stop();
        IEnumerator Animate();
    }
}