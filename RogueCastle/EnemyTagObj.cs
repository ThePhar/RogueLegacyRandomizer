/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using DS2DEngine;

namespace RogueCastle
{
	public class EnemyTagObj : GameObj
	{
		public string EnemyType
		{
			get;
			set;
		}
		protected override GameObj CreateCloneInstance()
		{
			return new EnemyTagObj();
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			EnemyTagObj enemyTagObj = obj as EnemyTagObj;
			enemyTagObj.EnemyType = EnemyType;
		}
	}
}
