using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Globe")]
	public class MegaGlobeWarp : MegaWarp
	{
		public float	dir			= -90.0f;
		public float	dir1		= -90.0f;
		public MegaAxis	axis		= MegaAxis.X;
		public MegaAxis	axis1		= MegaAxis.Z;
		Matrix4x4		mat			= new Matrix4x4();
		public bool		twoaxis		= true;
		Matrix4x4		tm1			= new Matrix4x4();
		Matrix4x4		invtm1		= new Matrix4x4();
		public float	r			= 0.0f;
		public float	r1			= 0.0f;
		public float	radius		= 10.0f;
		public bool		linkRadii	= true;
		public float	radius1		= 10.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string WarpName() { return "Globe"; }
		public override string GetHelpURL() { return "?page_id=3752"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				r;
			public float				r1;
			public bool					twoaxis;
			public float				totaldecay;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm1;
			public Matrix4x4			invtm1;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			wtm;
			public Matrix4x4			winvtm;

			public void Execute(int vi)
			{
				if ( r == 0.0f )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				float3 p = wtm.MultiplyPoint3x4(jvertices[vi]);

				p = tm.MultiplyPoint3x4(p);
				float3 ip = p;

				float dist = math.length(p);
				float dcy = math.exp(-totaldecay * math.abs(dist));

				float x = p.x;
				float y = p.y;

				float yr = (y / r);

				float c = math.cos(math.PI - yr);
				float s = math.sin(math.PI - yr);
				float px = r * c + r - x * c;
				p.x = px;
				float pz = r * s - x * s;
				p.y = pz;

				p = math.lerp(ip, p, dcy);

				p = invtm.MultiplyPoint3x4(p);

				if ( twoaxis )
				{
					p = tm1.MultiplyPoint3x4(p);

					x = p.x;
					y = p.y;

					yr = (y / r1);

					c = math.cos(math.PI - yr);
					s = math.sin(math.PI - yr);
					px = r1 * c + r1 - x * c;
					p.x = px;
					pz = r1 * s - x * s;
					p.y = pz;

					p = math.lerp(ip, p, dcy);
					p = invtm1.MultiplyPoint3x4(p);
				}

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.r			= r;
				job.r1			= r1;
				job.twoaxis		= twoaxis;
				job.totaldecay	= totaldecay;
				job.tm1			= tm1;
				job.invtm1		= invtm1;
				job.tm			= tm;
				job.invtm		= invtm;
				job.wtm			= mod.tm;
				job.winvtm		= mod.invtm;
				job.jvertices	= mod.jverts;
				job.jsverts		= mod.jsverts;

				jobHandle = job.Schedule(mod.jverts.Length, 64);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			if ( r == 0.0f )
				return p;

			p = tm.MultiplyPoint3x4(p);
			Vector3 ip = p;

			float dist = p.magnitude;
			float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

			float x = p.x;
			float y = p.y;

			float yr = (y / r);

			float c = Mathf.Cos(Mathf.PI - yr);
			float s = Mathf.Sin(Mathf.PI - yr);
			float px = r * c + r - x * c;
			p.x = px;
			float pz = r * s - x * s;
			p.y = pz;

			p = Vector3.Lerp(ip, p, dcy);

			p = invtm.MultiplyPoint3x4(p);

			if ( twoaxis )
			{
				p = tm1.MultiplyPoint3x4(p);

				x = p.x;
				y = p.y;

				yr = (y / r1);

				c = Mathf.Cos(Mathf.PI - yr);
				s = Mathf.Sin(Mathf.PI - yr);
				px = r1 * c + r1 - x * c;
				p.x = px;
				pz = r1 * s - x * s;
				p.y = pz;

				p = Vector3.Lerp(ip, p, dcy);
				p = invtm1.MultiplyPoint3x4(p);
			}
			return p;
		}

		void Calc()
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;

			mat = Matrix4x4.identity;

			tm1 = tm;
			invtm1 = invtm;

			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir);
			SetAxis(mat);

			mat = Matrix4x4.identity;

			switch ( axis1 )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir1);
			Matrix4x4 itm = mat.inverse;
			tm1 = mat * tm1;
			invtm1 = invtm1 * itm;

			r = -radius;

			if ( linkRadii )
				r1 = -radius;
			else
				r1 = -radius1;
		}

		public override bool Prepare(float decay)
		{
			Calc();

			totaldecay = Decay + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;

			return true;
		}
	}
}