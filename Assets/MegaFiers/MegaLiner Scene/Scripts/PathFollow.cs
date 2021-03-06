using UnityEngine;

namespace MegaFiers
{
	[ExecuteInEditMode]
	public class PathFollow : MonoBehaviour
	{
		public	float			tangentDist	= 0.01f;	// how far it looks ahead or behind to calc rotation
		public	float			alpha		= 0.0f;		// how far along curve as a percent
		public	float			speed		= 0.0f;		// how fast it moves
		public	bool			rot			= false;	// check if you want to change rotation
		public	float			time		= 0.0f;		// how long to take to travel whole shape (system checks UseDistance then time then speed for which method it chooses, set non used to 0)
		public	float			ctime		= 0.0f;		// current time for time animation
		public	int				curve		= 0;		// curve to use in shape
		public	MegaShape		target;					// Shape to follow
		public	float			distance	= 0.0f;		// distance along shape
		public	bool			animate		= false;	// automatically moves the object
		public	bool			UseDistance	= true;		// use distance method
		public	bool			addtwist	= false;
		public Vector3			offset		= Vector3.zero;
		public Vector3			rotate		= Vector3.zero;
		public MegaRepeatMode	loopmode	= MegaRepeatMode.Loop;

		public void SetPos(float a)
		{
			if ( target != null )
			{
				float twist = 0.0f;

				switch ( loopmode )
				{
					case MegaRepeatMode.Clamp: a = Mathf.Clamp01(a); break;
					case MegaRepeatMode.Loop: a = Mathf.Repeat(a, 1.0f); break;
					case MegaRepeatMode.PingPong: a = Mathf.PingPong(a, 1.0f); break;
				}

				Vector3 off = Vector3.zero;
				Vector3	pos = target.InterpCurve3D(curve, a, target.normalizedInterp, ref twist);

				if ( rot )
				{
					float ta = tangentDist / target.GetCurveLength(curve);
					Vector3 pos1 = target.InterpCurve3D(curve, a + ta, target.normalizedInterp);

					Vector3 rt = rotate;

					Quaternion tq = Quaternion.Euler(0.0f, 0.0f, twist);
					Quaternion er = Quaternion.Euler(rt);

					if ( addtwist )
						er = tq * er;

					Vector3 dir = pos1 - pos;
					Quaternion r = Quaternion.LookRotation(dir);
					Matrix4x4 tm = Matrix4x4.TRS(Vector3.zero, r * er, Vector3.one);
					off = tm.MultiplyPoint3x4(offset);

					transform.localRotation = r * er;
				}

				transform.position = target.transform.TransformPoint(pos - off);
			}
		}

		public void SetPosFomDist(float dist)
		{
			if ( target != null )
			{
				float a = dist / target.GetCurveLength(curve);
				float twist = 0.0f;

				switch ( loopmode )
				{
					case MegaRepeatMode.Clamp: a = Mathf.Clamp01(a); break;
					case MegaRepeatMode.Loop: a = Mathf.Repeat(a, 1.0f); break;
					case MegaRepeatMode.PingPong: a = Mathf.PingPong(a, 1.0f); break;
				}

				Vector3 off = Vector3.zero;
				Vector3 pos = target.InterpCurve3D(curve, a, target.normalizedInterp, ref twist);

				if ( rot )
				{
					float ta = tangentDist / target.GetCurveLength(curve);

					Vector3 pos1 = target.InterpCurve3D(curve, a + ta, target.normalizedInterp);
					Vector3 rt = rotate;

					Quaternion tq = Quaternion.Euler(0.0f, 0.0f, twist);
					Quaternion er = Quaternion.Euler(rt);

					if ( addtwist )
						er = tq * er;

					Vector3 dir = pos1 - pos;
					Quaternion r = Quaternion.LookRotation(dir);

					Matrix4x4 tm = Matrix4x4.TRS(Vector3.zero, r * er, Vector3.one);
					off = tm.MultiplyPoint3x4(offset);
					transform.localRotation = r * er;
				}

				transform.position = target.transform.TransformPoint(pos - off);
			}
		}

		public void Start()
		{
			ctime = 0.0f;
			curve = 0;
		}

		void Update()
		{
			if ( animate && Application.isPlaying )
			{
				if ( UseDistance )
					distance += speed * Time.deltaTime;
				else
				{
					if ( time > 0.0f )
					{
						ctime += Time.deltaTime;

						if ( ctime > time )
							ctime = 0.0f;

						alpha = (ctime / time) * 100.0f;
					}
					else
					{
						if ( speed != 0.0f )
						{
							alpha += speed * Time.deltaTime;

							if ( alpha > 100.0f )
								alpha = 0.0f;
							else
							{
								if ( alpha < 0.0f )
									alpha = 100.0f;
							}
						}
					}
				}
			}

			if ( UseDistance )
				SetPosFomDist(distance);
			else
				SetPos(alpha * 0.01f);
		}
	}
}