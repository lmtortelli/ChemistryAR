
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Vuforia;

public class TrackableController : MonoBehaviour
{
    public static Dictionary<string,Molecula> compoundMolecules;
    List<Molecula> nameGameObject;
    Dictionary<string, TrackableBehaviour> gameObjectTracked;
    bool h20 = false;
    bool co2 = false;
    bool threading = false;
    Dictionary<string, bool> isUsed;
    Dictionary<string, bool> isFirst;
    GameObject agua;
    GameObject dioxido;
    GameObject tree;


    void Start()
    {
        isUsed = new Dictionary<string, bool>();
        this.nameGameObject = new List<Molecula>();
        this.isFirst = new Dictionary<string, bool>();
        this.tree = GameObject.Find("Icosphere");
        TrackableController.compoundMolecules = new Dictionary<string, Molecula>();

    }


    public void Update()
    {
        IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetActiveTrackableBehaviours();

        List<Molecula> temp = new List<Molecula>();



        foreach (TrackableBehaviour tb in tbs)
        {
            if (tb.tag == "clear")
            {
                this.clearMap();
            }
            else
            {
                if (tb.tag != "tree")
                {
                    this.crossObject(new Molecula(tb));
                    this.checkChemistryReaction();
                }
                this.checkPhotosintesis();
            }
        }
    }

    private void checkPhotosintesis()
    {
        if (tree != null && h20 && co2) {
            var material = Resources.Load("treeStrong") as Material;
            tree.GetComponent<MeshRenderer>().material = material;  
            
        }
    }

    private void checkChemistryReaction()
    {
        Debug.Log("VERIFY REACTION");
        checkH20();
        checkCO2();

    }

    private void checkH20()
    {
        var th20 = new Molecula();
        List<Molecula> lh20 = new List<Molecula>();
        foreach (var element in this.nameGameObject)
        {
            if (!element.used && element != null)
            {
                if (!h20)
                { // Proura H20     
                    if (element.tb.tag == "hydrogen")
                    {
                        th20.hidr++;
                        lh20.Add(element);
                    }
                    else if (element.tb.tag == "oxygen")
                    {
                        th20.oxy++;
                        lh20.Add(element);
                    }
                    
                }
            }

            if ((th20.hidr == 2 && th20.oxy == 1) && !h20)
            {
                h20 = true;
                this.updateReactionElements("agua", lh20, th20);
            }

        }
    }

    private void checkCO2()
    {
        var tco2 = new Molecula();
        List<Molecula> lco2 = new List<Molecula>();
        foreach (var element in this.nameGameObject)
        {
            if (!element.used)
            {
                if (!co2)
                { // Proura co2
                    if (element.tb.tag == "oxygen")
                    {
                        tco2.oxy++;
                    }
                    else if (element.tb.tag == "carbon")
                    {
                        tco2.carb++;
                    }
                    lco2.Add(element);
                }

                //Occurs reaction
                if ((tco2.carb == 1 && tco2.oxy == 2) && !co2)
                {
                    co2 = true;
                    this.updateReactionElements("dioxido", lco2, tco2);
                }

            }

        }
    }

    private void updateReactionElements(string nameElement, List<Molecula> lElements, Molecula newElement)
    {

        //Carrega trackablebehavior ao novo elemento para poder ter acesso dentro do MoleculeController
        newElement.UpdateMolecule(nameElement, lElements[0].tb);


        // Ajusta os elementos para que possam ser escondidos e o novo elemento fique vinculado a um único card
        for(int i=0; i <lElements.Count; i++) {
            if (i != 0)
            {
                lElements[i].tb.gameObject.SetActive(false);
            }
            else {
                lElements[i].atomo.gameObject.SetActive(false);
            }
            lElements[i].used = true;
            this.isUsed.Add(lElements[i].name, true);
            this.isFirst.Add(lElements[i].name,i==0);
        }
       

        // Deixar generico
        if (nameElement.Equals("agua"))
        {
            this.agua = GameObject.Find(nameElement);
            this.agua.SetActive(true);

            var water = GameObject.Find(nameElement + "_group");
            water.transform.SetParent(lElements[0].tb.transform);
            water.transform.localPosition = new Vector3(0.0f, 0.2f, 0.0f);

            water.transform.localRotation = Quaternion.identity;
            water.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //this.agua.GetComponent<Renderer>().enabled = true;
            newElement.atomo = this.agua;
            
        }
        else
        {
            this.dioxido = GameObject.Find(nameElement);
            this.dioxido.SetActive(true);
            //this.dioxido.transform.SetParent(lElements[0].tb.transform);
            var dio = GameObject.Find(nameElement + "_group");
            dio.transform.SetParent(lElements[0].tb.transform);
            dio.transform.localPosition = new Vector3(0.0f, 0.2f, 0.0f);
            //this.agua.transform.position = lElements[0].atomo.transform.position;   
            dio.transform.localRotation = Quaternion.identity;
            dio.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            // this.dioxido.GetComponent<Renderer>().enabled = true;
            newElement.atomo = this.dioxido;

        }
        newElement.tb = lElements[0].tb;
        TrackableController.compoundMolecules.Add(nameElement, newElement);


    }

    private void clearMap()
    {
        foreach (var obj in this.nameGameObject)
        {
            //obj.atomo.GetComponent<Renderer>().enabled = true;
            obj.tb.gameObject.SetActive(true);
            obj.atomo.gameObject.SetActive(true);
            ///obj.tb.enabled = true;
        }
        this.nameGameObject.Clear();
        h20 = false;
        co2 = false;
        this.isUsed.Clear();
        this.isFirst.Clear();
        try
        {
            this.agua.transform.SetParent(null);
            this.agua.SetActive(true);
        }
        catch { }

        try
        {
            this.dioxido.transform.SetParent(null);
            this.dioxido.SetActive(true);
        }
        catch { }

        if (this.tree != null)
        {
            var material = Resources.Load("treeWeak") as Material;
            Debug.Log("MATERIAL: " + material);
            Debug.Log("LOP "+tree.GetComponent<MeshRenderer>().material.shader);   
            tree.GetComponent<MeshRenderer>().material = material;

        }

        Debug.Log("LIST ELEMENTOS: " + this.nameGameObject.Count);
        Debug.Log("LIST USADOS " + this.isUsed.Count);
        Debug.Log("LIST PRIMEIROS " + this.isFirst.Count);
    }

    // Se o objeto ja existe na listagem e caso algum tenha saido o retirar.
    private void crossObject(Molecula temp)
    {
        int find = 0;
        temp.used = this.isUsed.ContainsKey(temp.name) ? this.isUsed[temp.name] : false;
        if (temp.used && !this.isFirst[temp.name])
        {
            //temp.atomo.GetComponent<Renderer>().enabled = false;
            temp.tb.gameObject.SetActive(false);
            //temp.tb.enabled = false;
        }

        Debug.Log("QUANTIDADE LISTA:" + this.nameGameObject.Count);
        if (this.nameGameObject.Find(x => (x.tb.name == temp.name)) == null)
        {
            this.nameGameObject.Add(temp);
        }

    }
}