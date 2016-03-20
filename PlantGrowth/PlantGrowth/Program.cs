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
    enum plant_enum
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
    class Plant
    {
        private float x, y, width_radius, height;
        public int state;

        public Plant(float x, float y, int state)
        {
            this.x = x;
            this.y = y;
            this.state = state;
            this.width_radius = 0;
            this.height = 0;
        }

        //getters

        //may vary per plant type
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
            return color_rep_enum.WINTER;
        }
        public float get_height()
        {
            return this.height;
        }

        public float get_width_radius()
        {
            return this.width_radius;
        }
    }
    class BearGrass : Plant
    {
        public BearGrass(float x, float y, int state) : base(x, y, state)
        {
        }
    }

    class LittleBluestem : Plant
    {
        public LittleBluestem(float x, float y, int state) : base(x, y, state)
        {
        }
    }

    class Yucca : Plant
    {
        public Yucca(float x, float y, int state) : base(x, y, state)
        {
        }
    }

    class GrowthSimulation
    {
        private static int month;

        static void Main(string[] args)
        {
            List<Plant> plants = new List<Plant>();
            plants.Add(new BearGrass());
            plants.Add(new LittleBluestem());
            plants.Add(new Yucca());
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