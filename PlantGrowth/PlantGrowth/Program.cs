using KdTree;
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
        public const int MaxNeighbors = 10;
        public static float AverageAustinSunAngleFactor = (float)Math.Atan(25.9);
        public const float MaxSunOverlap = 88; // Inches of max shadow based on beargrass (largest plant)

        public Plant(float x, float y, int state)
        {
            this.x = x;
            this.y = y;
            this.state = (state_enum)state;
            if (state == (int)state_enum.SEED)
            {
                this.width_radius = 0;
                this.height = 0;
            }
            else
            {
                this.height = 4.33f;
                this.width_radius = 4;
            }
        }

        //getters

        public abstract color_rep_enum get_color_rep(month_enum month);

        public virtual float SunRequirementsPercentage()
        {
            return 33; // Partial and Full Sun
        }

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

        public float ShadowLength()
        {
            return width_radius + (height * Plant.AverageAustinSunAngleFactor);
        }

        public float DistanceSquared(Plant neighbor)
            {
            return (float)(Math.Pow(x - neighbor.x, 2) + Math.Pow(y - neighbor.y, 2));
        }

        public float GetSunScaler()
                {
            var neighbors = GrowthSimulation.plants_tree.RadialSearch(new float[] { this.x, this.y }, Plant.MaxSunOverlap, Plant.MaxNeighbors);
            float shadow_percentage = 0;
            foreach (var neighbor in neighbors)
            {
                var shadow_length = neighbor.Value.ShadowLength();
                var distance_squared = this.DistanceSquared(neighbor.Value);
                if (neighbor.Value != this && distance_squared < (shadow_length * shadow_length))
                {
                    shadow_percentage += (float)Math.Sqrt(distance_squared);
                }
            }
            shadow_percentage = Math.Min(shadow_percentage, 1) / width_radius;
            if(shadow_percentage < this.SunRequirementsPercentage())
            {
                return -1; // Magic Value -1 means plant died
            }
            return (float)((0.217147241) * Math.Log(shadow_percentage+1)); // http://www.solarpaneltilt.com/
        }

        public abstract float GetGrowth();

		public Changes simulate() 
		{
			Changes changes = new Changes(this);
			// TODO change values of new_height, new_radius, new_state
			
			
			return changes;
		}
    }

    class BearGrass : Plant
    {
        public static float minimum_radius = 18;
        public BearGrass(float x, float y, int state) : base(x, y, state)
        {
        }
		
		public override color_rep_enum get_color_rep(month_enum month)
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

        public override float SunRequirementsPercentage()
        {
            return 66; // Full Sun Only
        }

        public override float GetGrowth()
        {
            return 4.365827924 * Math.Log() - 2.901722353;
        }
    }

    class LittleBluestem : Plant
    {
        public static float minimum_radius = 4;
        public LittleBluestem(float x, float y, int state) : base(x, y, state)
        {
        }
		
		public override color_rep_enum get_color_rep(month_enum month)
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

        public override float GetGrowth()
        {
            throw new NotImplementedException();
        }
    }

    class Yucca : Plant
    {
        public static float minimum_radius = 4;
        public Yucca(float x, float y, int state) : base(x, y, state)
        {
        }
		
		public override color_rep_enum get_color_rep(month_enum month)
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

        public override float GetGrowth()
        {
            throw new NotImplementedException();
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
        public static List<KdTreeNode<float, Plant>> simulation_plants;
        public static KdTree<float, Plant> plants_tree;
        private static Queue<Changes> changes_queue;
        static void MainFunction(List<float> xpositions, List<float> ypositions, List<int> plant_states, List<int> plant_type, int iterations, List<float> plant_heights, List<float> plant_radius, List<float> plant_output_state)
        {
            KdTreeNode<float, Plant> tree_node;
            for (int i=0; i<xpositions.Count; ++i)
            {
                switch (plant_type[i])
                {
                    case (int)plant_enum.BEAR:
                        tree_node = new KdTreeNode<float, Plant>(new float[] { xpositions[i], ypositions[i] }, new BearGrass(xpositions[i], ypositions[i], plant_states[i]));
                        break;
                    case (int)plant_enum.YUCCA:
                        tree_node = new KdTreeNode<float, Plant>(new float[] { xpositions[i], ypositions[i] }, new Yucca(xpositions[i], ypositions[i], plant_states[i]));
                        break;
                    default:
                        tree_node = new KdTreeNode<float, Plant>(new float[] { xpositions[i], ypositions[i] }, new LittleBluestem(xpositions[i], ypositions[i], plant_states[i]));
                        break;
                }
                simulation_plants.Add(tree_node);
                plants_tree.Add(tree_node.Point, tree_node.Value);
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
            foreach (var plant_node in simulation_plants)
            {
                changes_queue.Enqueue(plant_node.Value.simulate());
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
            return (int)GrowthSimulation.month;
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
