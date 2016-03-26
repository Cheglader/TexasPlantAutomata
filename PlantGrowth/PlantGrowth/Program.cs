using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantGrowth
{
    enum month_enum
    {
        JAN,
        FEB,
        MAR,
        APR,
        MAY,
        JUN,
        JUL,
        AUG,
        SEP,
        OCT,
        NOV,
        DEC
    }
    enum state_enum
    {
        ALIVE,
        SEED,
        DEAD
    }
    enum color_rep_enum
    {
        WINTER,
        FLOWER,
        GREEN
    }
    enum sun_enum
    {
        SHADE,
        PARTIAL,
        FULL
    }
    enum plant_enum
    {
        BEAR,
        YUCCA,
        BLUESTEM
    }
    abstract class Plant
    {
        protected float x, y, width_radius, height;
        public state_enum state;
        protected const float min_radius = 99999;
        protected const sun_enum sun_requirements = sun_enum.FULL;

        public Plant(float x, float y, int state)
        {
            this.x = x;
            this.y = y;
            this.state = (state_enum)state;
            this.width_radius = 0;
            this.height = 0;
        }

        //getters

        //may vary per plant type
        public abstract color_rep_enum get_color_rep(month_enum month);

        public float get_height()
        {
            return this.height;
        }

        public float get_width_radius()
        {
            return this.width_radius;
        }

        public void NewDimensions(float new_height, float new_radius, state_enum new_state) 
        {
            this.height = new_height;
            this.width_radius = new_radius;
            this.state = new_state;
        }

        public float GetSunScaler(List<Plant> neighbors)
        {
            foreach (var neighbor in neighbors)
            {
                if(neighbor != self && Distance(neighbor, self) < neighbor.MaxShadowDistance)
                {
                    // TODO distance formula, MaxShadowDistance pseudo-constant
                }
            }
        }
		
		public Changes simulate(List<Plant> simulation_plants) 
		{
			Changes changes = new Changes(this);
			//change values of new_height, new_radius, new_state
			
			
			return changes;
		}
    }

    class BearGrass : Plant
    {
        public BearGrass(float x, float y, int state) : base(x, y, state)
        {
        }
		
		public color_rep_enum get_color_rep(month_enum month)
        {
            switch (month)
            {
                case month_enum.JAN:
                    return color_rep_enum.WINTER;
                case month_enum.FEB:
                    return color_rep_enum.WINTER;
                case month_enum.MAR:
                    return color_rep_enum.FLOWER;
                case month_enum.APR:
                    return color_rep_enum.FLOWER;
                case month_enum.MAY:
                    return color_rep_enum.FLOWER;
                case month_enum.JUN:
                    return color_rep_enum.FLOWER;
                case month_enum.JUL:
                    return color_rep_enum.FLOWER;
                case month_enum.AUG:
                    return color_rep_enum.GREEN;
                case month_enum.SEP:
                    return color_rep_enum.GREEN;
                case month_enum.OCT:
                    return color_rep_enum.GREEN;
                case month_enum.NOV:
                    return color_rep_enum.WINTER;
                case month_enum.DEC:
                    return color_rep_enum.WINTER;
            }
            return color_rep_enum.WINTER;
        }
    }

    class LittleBluestem : Plant
    {
        public LittleBluestem(float x, float y, int state) : base(x, y, state)
        {
        }
		
		public color_rep_enum get_color_rep(month_enum month)
        {
            switch (month)
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
                    return color_rep_enum.GREEN;
                case month_enum.JUN:
                    return color_rep_enum.FLOWER;
                case month_enum.JUL:
                    return color_rep_enum.FLOWER;
                case month_enum.AUG:
                    return color_rep_enum.FLOWER;
                case month_enum.SEP:
                    return color_rep_enum.FLOWER;
                case month_enum.OCT:
                    return color_rep_enum.FLOWER;
                case month_enum.NOV:
                    return color_rep_enum.FLOWER;
                case month_enum.DEC:
                    return color_rep_enum.FLOWER;
            }
            return color_rep_enum.WINTER;
        }
    }

    class Yucca : Plant
    {
        public Yucca(float x, float y, int state) : base(x, y, state)
        {
        }
		
		public color_rep_enum get_color_rep(month_enum month)
        {
            switch (month)
            {
                case month_enum.JAN:
                    return color_rep_enum.WINTER;
                case month_enum.FEB:
                    return color_rep_enum.WINTER;
                case month_enum.MAR:
                    return color_rep_enum.FLOWER;
                case month_enum.APR:
                    return color_rep_enum.FLOWER;
                case month_enum.MAY:
                    return color_rep_enum.FLOWER;
                case month_enum.JUN:
                    return color_rep_enum.FLOWER;
                case month_enum.JUL:
                    return color_rep_enum.FLOWER;
                case month_enum.AUG:
                    return color_rep_enum.GREEN;
                case month_enum.SEP:
                    return color_rep_enum.GREEN;
                case month_enum.OCT:
                    return color_rep_enum.GREEN;
                case month_enum.NOV:
                    return color_rep_enum.WINTER;
                case month_enum.DEC:
                    return color_rep_enum.WINTER;
            }
            return color_rep_enum.WINTER;
        }
    }
    class Changes
    {
        private Plant changer;
        private float new_height;
        private float new_radius;
        private state_enum new_state;
		
		public Changes(Plant plant) 
		{
			changer = plant;
		}
        public void ApplyChanges()
        {
            changer.NewDimensions(new_height, new_radius, new_state);
        }
    }
    class GrowthSimulation
    {
        private static int month;
        private static List<Plant> simulation_plants; // TODO change to quad tree
        private static Queue<Changes> changes_queue;
        static void MainFunction(List<float> xpositions, List<float> ypositions, List<int> plant_states, List<int> plant_type, int iterations, List<float> plant_heights, List<float> plant_radius, List<float> plant_output_state)
        {
            for(int i=0; i<xpositions.Count; ++i)
            {
                switch (plant_type[i])
                {
                    case (int)plant_enum.BEAR:
                        simulation_plants.Add(new BearGrass(xpositions[i], ypositions[i], plant_states[i]));
                        break;
                    case (int)plant_enum.YUCCA:
                        simulation_plants.Add(new Yucca(xpositions[i], ypositions[i], plant_states[i]));
                        break;
                    default:
                        simulation_plants.Add(new LittleBluestem(xpositions[i], ypositions[i], plant_states[i]));
                        break;
                }
            }
			
			//maybe this loop should go in grasshopper script, so C# script is run multiple times giving new output every time?
            for(int i=0; i<iterations; ++i)
            {
                GenerateChanges();
                ApplyChanges();
            }
			
			//modify variable plant_output_state
			
        }

        private static void GenerateChanges()
        {
            foreach (var plant in simulation_plants)
            {
                changes_queue.Enqueue(plant.simulate(simulation_plants));
            }
        }

        private static void ApplyChanges()
        {
            while(changes_queue.Any())
            {
                var head = changes_queue.Dequeue();
                head.ApplyChanges();
            }
        }

        public static int get_month()
        {
            return month;
        }
    }
    // Args: Iterations, x positions, y positions, states, 
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
