using System.Collections;
using System.Collections.Generic;

namespace app
{
    public class Connection
{
    public Node inputNode;
        System.Random rng = new System.Random();

    public double weight = 0;

    public Node outputNode;
    public int inputLayer;
    public bool endIsOutput;

    public Connection cloneConection(){
        Connection clonedCon = new Connection(inputNode,outputNode,inputLayer,endIsOutput,weight);
        return clonedCon;
    }
    public Connection(Node _inputNode,Node _outputNode,int _inputLayer,bool _endIsOutput,double _weight){
        this.inputNode=_inputNode;
        this.weight=_weight;
        this.outputNode=_outputNode;
        this.inputLayer=_inputLayer;
        this.endIsOutput=_endIsOutput;
    }
    public Connection(Node inputNode,Node outputNode,int _inputLayer,bool _endIsOutput){
        this.inputNode=inputNode;
        double randomValue = rng.NextDouble() * 2 - 1;
        weight = randomValue;
        this.outputNode=outputNode;
        this.inputLayer=_inputLayer;
        this.endIsOutput=_endIsOutput;
    }
    

    public double passData(double input){
        return input*weight;
    }


    public void transportData(){
        // now checked by the inputLayer
        /*if(inputNode.isReady&&!inputNode.dataAlrTaken)
        {
            
        }*/
        outputNode.collectData(passData(inputNode.outputData));
        //inputNode.dataAlrTaken=true;
        
    }



}

}
