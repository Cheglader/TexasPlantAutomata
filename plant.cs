
using System;
namespace PlantGrowth
{
    enum month_enum
    {
		JAN=0,
		FEB=1,
		MAR=2,
		APR=3,
		MAY=4,
		JUN=5,
		JUL=6,
		AUG=7,
		SEP=8,
		OCT=9,
		NOV=10,
		DEC=11
    }
	
	enum plant_enum
    {
		ALIVE=0,
		SEED=1,
		DEAD=2
    }
	
	enum color_rep_enum
    {
		WINTER=0,
		FLOWER=1,
		GREEN=2
    }
	
	enum sun_enum
    {
		SHADE=0,
		PARTIAL=1,
		FULL=2
    }
	
	class Plant
    {
		public float x, y, width_radius, height;
		public int state;
		
		public Plant(x, y, state)
		{
			this.x = x;
			this.y = y;
			this.state = state;
			this.width_radius = 0;
			this.height = 0;
		}
		
		//show outputs here
		public void color_rep() {
			
		}
		
		public void height() {
			
		}
		
		public void width_radius() {
			
		}
		
    }
	
	class BearGrass: Plant 
	{
		public BearGrass(x, y, state) : base(x, y, state)
		{
		}
	}
	
	class LittleBluestem: Plant 
	{
		public LittleBluestem(x, y, state) : base(x, y, state)
		{
		}
	}
	
	class Yucca: Plant 
	{
		public Yucca(x, y, state) : base(x, y, state)
		{
		}
	}

}