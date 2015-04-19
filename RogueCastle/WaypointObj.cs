using DS2DEngine;

namespace RogueCastle
{
	public class WaypointObj : GameObj
	{
		public int OrbType;
		protected override GameObj CreateCloneInstance()
		{
			return new WaypointObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			WaypointObj waypointObj = obj as WaypointObj;
			waypointObj.OrbType = OrbType;
		}
	}
}
