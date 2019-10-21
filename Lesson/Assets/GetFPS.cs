using UnityEngine;
using UnityEngine.Profiling;

namespace GeekBrains
{
	public class GetFPS : MonoBehaviour
	{
		public Cloth Cloth;
		private Animator anim;
		private static readonly int Trigger = Animator.StringToHash("Trigger");

		private void OnValidate()
		{
			Profiler.BeginSample("wwetfwefwegbgwehbtfrgerebhhwef ");
			anim.SetInteger(Trigger,56);
			Cloth = GameObject.FindObjectOfType<Cloth>();
			Profiler.EndSample();
		}
/*
		private void Update()
		{
			print(1 / Time.deltaTime);

			if(!Input.GetKeyDown(KeyCode.Space))return;
			Cloth.SetEnabledFading(false);
		}*/
		
		public void Test()
		{
			print(1 / Time.deltaTime);

            if (Time.frameCount % 2 == 0)
            {

            }
            else
            {
                
            }
		}
	}
}