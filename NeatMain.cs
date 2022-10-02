using System;
using System.Collections;
using System.Collections.Generic;
namespace app
{
    public class NeatMain

{   
    

    System.Random rng = new System.Random();
    
    public Node[] Inputnodes;
    public Node[] Outputnodes;
    public List<Node> HiddenNodes = new List<Node>();
    public List<Connection> connections = new List<Connection>();
    public int numOffHiddenLayers = 0;
    double[] outputs;
    public double lastPerformance;
    public NeatMain(int InputAmount,int NumOutPuts){
       Inputnodes = new Node[InputAmount]; 
       Outputnodes = new Node[NumOutPuts];
       outputs = new double[NumOutPuts]; 
       fillNodes();
    }

    public NeatMain CloneMe(){
        NeatMain clone = new NeatMain(Inputnodes.Length,Outputnodes.Length);
        List<Connection> connectionsClone = new List<Connection>();
        foreach (Connection con in connections)
        {
            connectionsClone.Add(con.cloneConection());
        }
        clone.connections = connectionsClone;
        List<Node> HiddenNodesClone = new List<Node>();
        foreach (Node nod in HiddenNodes)
        {
            HiddenNodesClone.Add(nod.cloneNode());
        }
        clone.HiddenNodes = HiddenNodesClone;
        clone.numOffHiddenLayers = this.numOffHiddenLayers;
        clone.lastPerformance = this.lastPerformance;
        
        int i = 0;
        foreach (Node nod in Outputnodes)
        {
            clone.Outputnodes[i]=nod.cloneNode();
            i++;
        }

        
        return clone;

    }

    

    public void fillNodes(){
        for (int i = 0; i < Inputnodes.Length; i++)
        {
            //Console.WriteLine("bei fill!");
            Inputnodes[i]= new Node(0);
        }

        for (int i = 0; i < Outputnodes.Length; i++)
        {
            Outputnodes[i]= new Node(99);
        }
    }

    public double[] calculate(double[] inputs)
    {
        //Debug.Log("!");
        int y = 0;
        
        foreach (Node InNode in Inputnodes)
        {
            InNode.safeData(inputs[y]);
            y++;
        }

        //***
        for (int i = 0; i <= numOffHiddenLayers; i++)
        {
            foreach (Connection con in connections)
            {
                if (con.inputLayer==0)
                {
                  con.transportData();  
                }
                
            }
        }
        
        //***



        for (int i = 1; i <= numOffHiddenLayers; i++)
        {
            foreach (Node hidden in HiddenNodes)
            {
                if (hidden.myLayer==i)
                {
                   hidden.valueData(); 
                }
                
            }
            foreach (Connection con in connections)
            {
                if (con.inputLayer==i)
                {
                    con.transportData();
                }
                
            }
        
        }
        y=0;
        foreach (Node Outputnode in Outputnodes)
        {
            Outputnode.valueData();
            outputs[y]=Outputnode.outputData;
            y++;
        }
        return outputs;

    }

    public double cost(DataPoint Data){
        double cost = 0;
        
        double[] values = calculate(Data.inputs);
        for (int i = 0; i < values.Length; i++)
        {
            double error = values[i]-Data.expectedOutputs[i];
            error = error*error;
            cost += error;
        }
        return cost;

    }

    public double valueAll(DataPoint[] learnData){
        double NetPerformance = 0;
        foreach (DataPoint data in learnData)
        {
            NetPerformance += cost(data);
        }
        lastPerformance = NetPerformance/learnData.Length;
        return NetPerformance/learnData.Length;
    }

    public void addConnection(Node from,Node to){
        bool toIsOutput = false;
        if (from.myLayer==to.myLayer)
        {
            return;
        }

        if (to.myLayer==99)
        {
            toIsOutput=true;
        }
        connections.Add(new Connection(from,to,from.myLayer,toIsOutput));


        /*bool isTargetHidden = false;
        int target = to;
        if(to>Outputnodes.Length){
            target -= Outputnodes.Length;
            isTargetHidden = true;
        }
        bool isSourceHidden = false;
        int source = from;
        if(from>Inputnodes.Length){
            source -= Inputnodes.Length;
            isSourceHidden = true;
        }

        if(isTargetHidden&&isSourceHidden){
           connections.Add(new Connection(HiddenNodes[source],HiddenNodes[target])); 
        }
        if(!isTargetHidden&&isSourceHidden){
           connections.Add(new Connection(HiddenNodes[source],Outputnodes[target])); 
        }
        if(isTargetHidden&&!isSourceHidden){
           connections.Add(new Connection(Inputnodes[source],HiddenNodes[target])); 
        }
        if(!isTargetHidden&&!isSourceHidden){
            connections.Add(new Connection(Inputnodes[source],Outputnodes[target]));    
        }*/
    }

    public void addNode(Node from,Node to){
        Node creatNode = new Node(from.myLayer+1);
        HiddenNodes.Add(creatNode);
        if (from.myLayer+1>numOffHiddenLayers)
        {
            numOffHiddenLayers++;
        }
        
        addConnection(from,creatNode);
        addConnection(creatNode,to);
        deleteConnection(from,to);
    }

    public void deleteConnection(Node from,Node to){
       Connection deleteMe;
       foreach (Connection con in connections)
            {
              if(con.inputNode==from&&con.outputNode==to){
                deleteMe = con;
                connections.Remove(deleteMe);
                break;
              }
            }
            
         
    }
    public void deleteConnection(int from,int to){
        deleteConnection(findInNode(from),findOutNode(to));
    }

    public void deleteALLNODEConnection(Node toDelete){
       Connection deleteMe;
       foreach (Connection con in connections)
            {
              if(con.inputNode==toDelete||con.outputNode==toDelete){
                deleteMe = con;
                connections.Remove(deleteMe);
                
              }
            }
            
         
    }
    
    /*public void deleteALLNODEConnection(int delet,int to){
        deleteALLNODEConnection(findInNode(from),findOutNode(to));
    }*/

    public void deleteHiddenNode(Node removeMe){
        deleteALLNODEConnection(removeMe);
        HiddenNodes.Remove(removeMe);
    }

    public void changeBias(Node node,double amount){
        node.bias += amount;
    }
    public void changeWeight(Connection con,double amount){
        con.weight += amount;
    }


    public Node findInNode(int Index){
        
        int source = Index;
        if(Index>=Inputnodes.Length){
            source -= Inputnodes.Length;
            return HiddenNodes[source];
        }
        return Inputnodes[source];
    }
    public Node findOutNode(int Index){
        
        int source = Index;
        if(Index>=Outputnodes.Length){
            source -= Outputnodes.Length;
            return HiddenNodes[source];
        }
        return Outputnodes[source];
    }

    public void mutateNet(int mutationOdds){

        

        if(rng.Next(100)<mutationOdds){
            addConnection(findInNode(rng.Next(Inputnodes.Length+HiddenNodes.Count)),findOutNode(rng.Next(Outputnodes.Length+HiddenNodes.Count)));
        }




        if(rng.Next(100)<mutationOdds&&connections.Count>2){
            
            addNode(findOutNode(rng.Next(Inputnodes.Length+HiddenNodes.Count)),findOutNode(rng.Next(Outputnodes.Length+HiddenNodes.Count)));
            
        }
        for (int i = 0; i < 3+HiddenNodes.Count; i++)
        {
            if(rng.Next(100)<mutationOdds){
                changeBias(findInNode(rng.Next(Inputnodes.Length+HiddenNodes.Count)),rng.NextDouble()*2-1);
            }
            if(rng.Next(100)<mutationOdds){
                changeBias(findOutNode(rng.Next(Outputnodes.Length+HiddenNodes.Count)),rng.NextDouble()*2-1);
            }
        }
        for (int i = 0; i < 6+HiddenNodes.Count; i++)
        {
            
            if(rng.Next(100)<mutationOdds&&connections.Count>1){
              //Debug.Log("190 Main");  
            changeWeight(connections[rng.Next(connections.Count-1)],rng.NextDouble()*2-1);
            }
            
            
        }

        try{
            if(rng.Next(100)<mutationOdds*0.5&&HiddenNodes.Count>1){
            deleteHiddenNode(HiddenNodes[rng.Next(HiddenNodes.Count-1)]);
        }

        }catch(InvalidOperationException exception){
            //Debug.Log("Error Catch");
        }
        
        
    }




}
}
