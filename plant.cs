
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
		private float x, y, width_radius, height;
		public int state;
		
		public Plant(x, y, state)
		{
			this.x = x;
			this.y = y;
			this.state = state;
			this.width_radius = 0;
			this.height = 0;
		}
		
		//getters
		
		//may vary per plant type
		public int get_color_rep(int month) {
			switch(month) 
			{
				case month_enum.JAN:
					return color_rep_enum.WINTER;
				case month_enum.FEB:
					return color_rep_enum.WINTER;
				case month_enum.MAR:
					return color_rep_enum.WINTER;
				case month_enum.APR:
					return color_rep_enum.GREEN;
				case month_enum.MAY:
					return color_rep_enum.FLOWER;
				case month_enum.JUN:
					return color_rep_enum.FLOWER;
				case month_enum.JUL:
					return color_rep_enum.FLOWER;
				case month_enum.AUG:
					return color_rep_enum.FLOWER;
				case month_enum.SEP:
					return color_rep_enum.GREEN;
				case month_enum.OCT:
					return color_rep_enum.GREEN;
				case month_enum.NOV:
					return color_rep_enum.WINTER;
				case month_enum.DEC:
					return color_rep_enum.WINTER;
			}
		}
		
		public float get_height() {
			return this.height;
		}
		
		public float get_width_radius() {
			return this.width_radius;
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
	
	class GrowthSimulation
	{
		private int month;
		
		static void Main(string[] args) 
        {
			List<Plant> plants = new List<Plant>();
			plants.add(new BearGrass());
			plants.add(new LittleBluestem());
			plants.add(new Yucca());
        }
		
		public static void get_month() {
			return month;
		}
	}

}