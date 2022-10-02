using System;
using System.Collections;
using System.Collections.Generic;

using static System.Math;
namespace app
{
    public class Node
{
    public int myLayer;
    public double inputCollection;
    public double inputCollectionwBias;
    
    public int activation = 0;

    public double bias;

    public double outputData;

    public bool isReady = false;
    public bool dataAlrTaken = false;
    
    public Node(int layer){
        myLayer=layer;
        if (myLayer==99)
        {
            activation = 1;
        }
    }

    public Node cloneNode(){
        Node clone = new Node(myLayer);
        clone.bias=this.bias;
        clone.activation=this.activation;
        return clone;
    }

    public void safeData(double input){
        outputData = input;
        isReady = true; 
    }

    public void collectData(double input){
        inputCollection += input; 
    }

    public void valueData(){
        
            inputCollectionwBias = inputCollection+bias;
            if(activation == 0){
                //L
                //outputData = Max(0, inputCollectionwBias);
                outputData =  1.0 / (1 + Exp(-inputCollectionwBias));
            }
            if (activation == 1)
            {
              outputData =  1.0 / (1 + Exp(-inputCollectionwBias));
               
            }
            
        
    }

    public void cleanNode(){
        inputCollection = 0;
        inputCollectionwBias=0;
        isReady = false;
        dataAlrTaken = false;
    }

    
    





}

}