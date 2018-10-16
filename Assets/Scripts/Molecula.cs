using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

public class Molecula
{
    public bool used { get; set; }
    public TrackableBehaviour tb { get; set; }
    public GameObject atomo;
    public string name;
    public int hidr { get; set; }
    public int oxy { get; set; }
    public int carb { get; set; }

    public Molecula(TrackableBehaviour obj) {
        this.tb = obj;
        this.name = obj.name;
        this.used = false;
        this.atomo = GameObject.Find(tb.name + "_a");
    }

    public Molecula() {
        this.used = false;
    }

    public void UpdateMolecule(string nameElement, TrackableBehaviour obj) {
        this.tb = obj;
        this.name = nameElement;
        this.used = false;
        this.atomo = GameObject.Find(nameElement);
    }
}

