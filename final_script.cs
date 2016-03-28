using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Collections;

using GH_IO;
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;


using KdTree;
using KdTree.Math;



/// <summary>
/// This class will be instantiated on demand by the Script component.
/// </summary>
public class Script_Instance : GH_ScriptInstance
{
#region Utility functions
  /// <summary>Print a String to the [Out] Parameter of the Script component.</summary>
  /// <param name="text">String to print.</param>
  private void Print(string text) { /* Implementation hidden. */ }
  /// <summary>Print a formatted String to the [Out] Parameter of the Script component.</summary>
  /// <param name="format">String format.</param>
  /// <param name="args">Formatting parameters.</param>
  private void Print(string format, params object[] args) { /* Implementation hidden. */ }
  /// <summary>Print useful information about an object instance to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj) { /* Implementation hidden. */ }
  /// <summary>Print the signatures of all the overloads of a specific method to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj, string method_name) { /* Implementation hidden. */ }
#endregion

#region Members
  /// <summary>Gets the current Rhino document.</summary>
  private readonly RhinoDoc RhinoDocument;
  /// <summary>Gets the Grasshopper document that owns this script.</summary>
  private readonly GH_Document GrasshopperDocument;
  /// <summary>Gets the Grasshopper script component that owns this script.</summary>
  private readonly IGH_Component Component;
  /// <summary>
  /// Gets the current iteration count. The first call to RunScript() is associated with Iteration==0.
  /// Any subsequent call within the same solution will increment the Iteration count.
  /// </summary>
  private readonly int Iteration;
#endregion

  /// <summary>
  /// This procedure contains the user code. Input parameters are provided as regular arguments,
  /// Output parameters as ref arguments. You don't have to assign output parameters,
  /// they will have a default value.
  /// </summary>
  private void RunScript(List<double> xpositions, List<double> ypositions, List<int> plant_states, List<int> plant_type, int iterations, ref object plant_radius, ref object plant_heights, ref object plant_output_state, ref object plant_color)
  {
    KdTreeNode<float, Plant> tree_node;
    List<float> xPos = xpositions.Select<double, float>(i => (float) i).ToList();
    List<float> yPos = ypositions.Select<double, float>(i => (float) i).ToList();
    List<int> pl_states = plant_states;
    List<int> pl_type = plant_type;
    int iter = Convert.ToInt32(iterations);
    List<float> plant_radius2 = new List<float>();
    List<float> plant_heights2 = new List<float>();
    List<int> plant_output_state2 = new List<int>();
    List<int> plant_color2 = new List<int>();
    GrowthSimulation.simulation_plants = new List<KdTreeNode<float, Plant>>();
    GrowthSimulation.plants_tree = new KdTree<float, Plant>(2, new FloatMath());
    for (int i = 0; i < xPos.Count; ++i)
    {
      switch (pl_type[i])
      {
        case (int) plant_enum.BEAR:
          tree_node = new KdTreeNode<float, Plant>(new float[] { xPos[i], yPos[i] }, new BearGrass(xPos[i], yPos[i], pl_states[i]));
          break;
        case (int) plant_enum.YUCCA:
          tree_node = new KdTreeNode<float, Plant>(new float[] { xPos[i], yPos[i] }, new Yucca(xPos[i], yPos[i], pl_states[i]));
          break;
        default:
          tree_node = new KdTreeNode<float, Plant>(new float[] { xPos[i], yPos[i] }, new LittleBluestem(xPos[i], yPos[i], pl_states[i]));
          break;
      }
      GrowthSimulation.simulation_plants.Add(tree_node);
      GrowthSimulation.plants_tree.Add(tree_node.Point, tree_node.Value);
    }

    //maybe this loop should go in grasshopper script, so C# script is run multiple times giving new output every time?
    for(int i = 0; i < iter; ++i)
    {
      ++Script_Instance.GrowthSimulation.month;
      GrowthSimulation.GenerateChanges();
      GrowthSimulation.ApplyChanges();
    }

    //modify variable plant_output_state
    foreach (var plant_node in GrowthSimulation.simulation_plants) {
      plant_heights2.Add(plant_node.Value.get_height());
      plant_radius2.Add(plant_node.Value.get_width_radius());
      plant_output_state2.Add((int) plant_node.Value.get_state());
      plant_color2.Add((int) plant_node.Value.get_color_rep());
    }
    plant_radius = plant_radius2;
    plant_heights = plant_heights2;
    plant_output_state = plant_output_state2;
    plant_color = plant_color2;
  }

  // <Custom additional code> 


  public enum month_enum
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
  public enum state_enum
  {
    ALIVE,
    SEED,
    DEAD
  }
  public enum color_rep_enum
  {
    WINTER,
    FLOWER,
    GREEN
  }
  public enum sun_enum
  {
    SHADE,
    PARTIAL,
    FULL
  }
  public enum plant_enum
  {
    BEAR,
    YUCCA,
    BLUESTEM
  }
  public abstract class Plant
  {
    protected float x, y, width_radius, height;
    protected int age;
    public state_enum state;
    public const int MaxNeighbors = 10;
    public static float AverageAustinSunAngleFactor = (float) Math.Atan(25.9);
    public const float MaxSunOverlap = 88; // Inches of max shadow based on beargrass (largest plant)

    public Plant(float x, float y, int state)
    {
      this.x = x;
      this.y = y;
      this.state = (state_enum) state;
      if (state == (int) state_enum.SEED)
      {
        this.width_radius = 0;
        this.height = 0;
        this.age = 0;
      }
      else
      {
        this.height = 4.33f; // Useless
        this.width_radius = 4; // Useless
        this.age = 5;
      }
    }

    //getters

    public abstract color_rep_enum get_color_rep();
    public abstract float MinSize();
    public abstract float HToWRatio();
    public abstract float MaxHeight();
    public abstract float MaxWidth();

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

    public state_enum get_state() {
      return state;
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
      return (float) (Math.Pow(x - neighbor.x, 2) + Math.Pow(y - neighbor.y, 2));
    }

    public float GetSunScaler()
    {
      var neighbors = GrowthSimulation.plants_tree.RadialSearch(new float[] { this.x, this.y }, Plant.MaxSunOverlap, Plant.MaxNeighbors);
      float shadow_percentage = 0;
      foreach (var neighbor in neighbors)
      {
        var r = neighbor.Value.ShadowLength(); // Shadow Length
        var R = width_radius;
        var distance_squared = this.DistanceSquared(neighbor.Value);
        var d = (float) Math.Sqrt(distance_squared);
        if(R < r){
          R = r;
          r = width_radius;
        }
        float part1 = (float) (r * r * Math.Acos((d * d + r * r - R * R) / (2 * d * r)));
        float part2 = (float) (R * R * Math.Acos((d * d + R * R - r * r) / (2 * d * R)));
        float part3 = (float) (0.5 * Math.Sqrt((-d + r + R) * (d + r - R) * (d - r + R) * (d + r + R)));

        float intersectionArea = part1 + part2 - part3;

        if (d < this.MinSize()) {
          return -1;
        }
        if (neighbor.Value != this && d < r)
        {
          shadow_percentage += intersectionArea / this.BaseArea();
        }
      }
      shadow_percentage = (float) (1 - (Math.Min(shadow_percentage, width_radius) / width_radius));
      if(shadow_percentage < this.SunRequirementsPercentage())
      {
        return -1; // Magic Value -1 means plant died
      }
      return (float) ((2.17147241) * Math.Log(shadow_percentage + 1)); // http://www.solarpaneltilt.com/
    }

    public float BaseArea()
    {
      return (float) (Math.PI * width_radius * width_radius);
    }

    public abstract float GetGrowth();

    public Changes simulate()
    {
      Changes changes;
      ++this.age;
      var total_growth = this.GetGrowth();
      //if (total_growth < 0) {
      //  changes = new Changes(this, 0, 0, state_enum.DEAD);
      //} else
      //{
      changes = new Changes(this, Math.Min(total_growth * (1 - this.HToWRatio()), this.MaxHeight()), Math.Min(total_growth * this.HToWRatio(), this.MaxWidth()), state_enum.ALIVE);
      //}
      return changes;
    }
  }

  public class BearGrass : Plant
  {
    public static float minimum_radius = 18;
    public static float MaxHeight2 = 72;
    public static float MaxWidth2 = 30;
    public static float HeightToWidthRatio = 0.41666664f;
    public static int bloom_years = 5;
    public BearGrass(float x, float y, int state) : base(x, y, state)
    {
      this.height = (float) (4.365827924 * Math.Log(age * 4 + 4) - 6.901722353);
      this.width_radius = this.height * this.HToWRatio();
    }

    public override color_rep_enum get_color_rep()
    {
      int inner_month = GrowthSimulation.month % 12;
      color_rep_enum expected_color = color_rep_enum.GREEN;
      switch ((month_enum) inner_month)
      {
        case month_enum.JAN:
          expected_color = color_rep_enum.WINTER;
          break;
        case month_enum.FEB:
          expected_color = color_rep_enum.WINTER;
          break;
        case month_enum.MAR:
          expected_color = color_rep_enum.FLOWER;
          break;
        case month_enum.APR:
          expected_color = color_rep_enum.FLOWER;
          break;
        case month_enum.MAY:
          expected_color = color_rep_enum.FLOWER;
          break;
        case month_enum.JUN:
          expected_color = color_rep_enum.FLOWER;
          break;
        case month_enum.JUL:
          expected_color = color_rep_enum.FLOWER;
          break;
        case month_enum.AUG:
          expected_color = color_rep_enum.GREEN;
          break;
        case month_enum.SEP:
          expected_color = color_rep_enum.GREEN;
          break;
        case month_enum.OCT:
          expected_color = color_rep_enum.GREEN;
          break;
        case month_enum.NOV:
          expected_color = color_rep_enum.WINTER;
          break;
        case month_enum.DEC:
          expected_color = color_rep_enum.WINTER;
          break;
      }
      if(expected_color == color_rep_enum.FLOWER && GrowthSimulation.GetYear() % bloom_years != 0) {
        return color_rep_enum.GREEN;
      }
      return color_rep_enum.WINTER;
    }

    public override float SunRequirementsPercentage()
    {
      return 66; // Full Sun Only
    }

    public override float GetGrowth()
    {
      float sun_scaler = this.GetSunScaler();
      //if(sun_scaler < 0) return -1;
      return (float) (Math.Max((4.365827924 * Math.Log(age * 4 + 4) - 6.901722353), 0) * sun_scaler);
    }

    public override float MinSize()
    {
      return minimum_radius;
    }

    public override float HToWRatio()
    {
      return HeightToWidthRatio;
    }

    public override float MaxHeight()
    {
      return MaxHeight2;
    }

    public override float MaxWidth()
    {
      return MaxWidth2;
    }
  }

  public class LittleBluestem : Plant
  {
    public static float minimum_radius = 4;
    public static float MaxHeight2 = 48;
    public static float MaxWidth2 = 18;
    public static float HeightToWidthRatio = 0.25f;
    public static Random RandomInstance = new Random();
    public LittleBluestem(float x, float y, int state) : base(x, y, state)
    {
      this.height = (float) (8.336309999 * Math.Log(age * 4 + 4) - 11.55657954);
      this.width_radius = this.height * this.HToWRatio();
    }

    public override color_rep_enum get_color_rep()
    {
      int inner_month = GrowthSimulation.month % 12;
      switch ((month_enum) inner_month)
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
      float sun_scaler = this.GetSunScaler();
      // if(sun_scaler < 0) return -1;
      return (float) ((4.365827924 * Math.Log(age * 4 + 4) - 2.901722353) * sun_scaler);
    }

    public override float MinSize()
    {
      return minimum_radius;
    }

    public override float HToWRatio()
    {
      return HeightToWidthRatio;
    }

    public override float MaxHeight()
    {
      return MaxHeight2;
    }

    public override float MaxWidth()
    {
      return MaxWidth2;
    }
  }

  public class Yucca : Plant
  {
    public static float minimum_radius = 4;
    public static float MaxHeight2 = 48;
    public static float MaxWidth2 = 12;
    public static float HeightToWidthRatio = 0.375f;
    public Yucca(float x, float y, int state) : base(x, y, state)
    {
      this.height = (float) (5.683847725 * Math.Log(age * 4 + 4) - 7.879486051);
      this.width_radius = this.height * this.HToWRatio();
    }

    public override color_rep_enum get_color_rep()
    {
      int inner_month = GrowthSimulation.month % 12;
      switch ((month_enum) inner_month)
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
      float sun_scaler = this.GetSunScaler();
      // if(sun_scaler < 0) return -1;
      return (float) ((4.365827924 * Math.Log(age * 4 + 4) - 2.901722353) * sun_scaler);
    }

    public override float MinSize()
    {
      return minimum_radius;
    }

    public override float HToWRatio()
    {
      return HeightToWidthRatio;
    }

    public override float MaxHeight()
    {
      return MaxHeight2;
    }

    public override float MaxWidth()
    {
      return MaxWidth2;
    }
  }
  public class Changes
  {
    private Plant changer;
    private float new_height;
    private float new_radius;
    private state_enum new_state;

    public Changes(Plant plant, float nheight, float nradius, state_enum nstate)
    {
      changer = plant;
      new_height = nheight;
      new_radius = nradius;
      new_state = nstate;
    }
    public void ApplyChanges()
    {
      changer.NewDimensions(new_height, new_radius, new_state);
    }
  }

  public class GrowthSimulation
  {
    public static int month;
    public static List<KdTreeNode<float, Plant>> simulation_plants = new List<KdTreeNode<float, Plant>>();
    public static KdTree<float, Plant> plants_tree = new KdTree<float, Plant>(2, new FloatMath());
    public static Queue<Changes> changes_queue = new Queue<Changes>();

    public static void GenerateChanges()
    {
      foreach (var plant_node in simulation_plants)
      {
        changes_queue.Enqueue(plant_node.Value.simulate());
      }
    }

    public static void ApplyChanges()
    {
      while(changes_queue.Any())
      {
        var head = changes_queue.Dequeue();
        head.ApplyChanges();
      }
    }

    public static int get_month()
    {
      return (int) GrowthSimulation.month;
    }

    public static int GetYear()
    {
      return (int) (GrowthSimulation.month / 12);
    }
  }
  // </Custom additional code> 
}
