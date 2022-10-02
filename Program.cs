using System;

namespace app
{
    class Program
    {
        static List<NeatMain> Brains = new List<NeatMain>();
        
        static void Main(string[] args)
        {
            DataPoint[] trainData = creatData(300);
            
            /*for (int i = 0; i < 784; i++)
            {
                Console.WriteLine(trainData[11].inputs[i]);
            } */
            
            
            
            

            for (int i = 0; i < 1000; i++)
                        {
                           evokution(1000,20,10000,batchMaker(trainData,10),800);
                            double cost = 0;
                            double mycost;
                            double minima = 100;
                            foreach (NeatMain Brain in Brains)
                            {
                                mycost = Brain.valueAll(trainData);
                                cost += mycost;
                                if(mycost<minima){
                                    minima = mycost;
                                }
                            }
                            Console.WriteLine(cost/10000);
                            Console.WriteLine(minima);
                            

                        }
        for (int i = 0; i < trainData.Length; i++)
        {
            foreach (NeatMain Brain in Brains)
                            {
                                Console.WriteLine(trainData[i].inputs[0]);
                                Console.WriteLine(trainData[i].inputs[1]);
                                Console.WriteLine(Brain.calculate(trainData[i].inputs)[0]);
                                Console.WriteLine(Brain.calculate(trainData[i].inputs)[1]);
                                Console.WriteLine(trainData[i].expectedOutputs[0]);
                                Console.WriteLine(trainData[i].expectedOutputs[1]);
                            }


        }
            

            

            //testNet.valueAll(trainData);
           //firstNetwork.classifyAll(creatDataPicturesTest());
           //Console.WriteLine(firstNetwork.cost(trainData));
          
            //firstNetwork.Learn(creatData(),0.0001);
            //firstNetwork.Learn(creatData(),0.0001);
            
           /* for (int i = 0; i < 100; i++)
            {
                DataPoint[] testData = batchMaker(trainData,100);
                firstNetwork.Learn(testData,2);
                if(i%500 == 0){
                    Console.WriteLine(firstNetwork.cost(testData));
                }
                
            }

            foreach (DataPoint data in realTestData)
            {
                Console.WriteLine(firstNetwork.Classify(data.inputs));
                Console.WriteLine("real:" + data.label);

            }
            Console.WriteLine(firstNetwork.cost(realTestData));

*/
            

            
            

        

        }

        

        public static void removeBadBrains(DataPoint[] dataPoints,int whenToKill,int topWhat,int maxBrainCount){
            double[] valueAt = new double[Brains.Count];
            
            for (int i = 0; i < Brains.Count; i++)
            {
                valueAt[i]=Brains[i].valueAll(dataPoints);
                valueAt[i]+= 0.01*Brains[i].HiddenNodes.Count;
            }
            Array.Sort(valueAt);
            
            int BrainCount = Brains.Count;
            
            for (int i = 0; i < BrainCount; i++)
            {
                //valueAt[800]>Brains[i].lastPerformance&&
               // Console.WriteLine(Brains.Count);
                if(Brains.Count>=3000&&valueAt[1000]>Brains[i].lastPerformance&&Brains.Count<=maxBrainCount&&Brains[i].lastPerformance>0)
                {
                    //Console.WriteLine("Brains.Count");
                    Brains.Add(Brains[i].CloneMe());
                    
                    
                }  
                if (valueAt[whenToKill]<Brains[i].lastPerformance&&Brains.Count>=300)
                {
                    Brains.RemoveAt(i);
                    
                    i--;
                    BrainCount--;
                }    
            }
            

            
        
        
            //Brains.AddRange(TopBrains);*/
        }

        public static void evokution(int epochAmounts,int mutationRate,int brainCount,DataPoint[] dataPoints,int whenToKill){
            for (int i = 0; i < epochAmounts; i++)
            {
                keepAt(brainCount);
                mutate(mutationRate);
                removeBadBrains(dataPoints,whenToKill,100,11000);
                //repoduceGoodStuff(dataPoints,100,1000);                
            }
        }

        public static void keepAt(int amount){
            if (amount>Brains.Count)
            {
                Brains.Add(new NeatMain(2,2));
                keepAt(amount);
                
            }
            else
            {
                return;
            }
        }
        public static void mutate(int rate){
            foreach (NeatMain Brain in Brains)
            {
                Brain.mutateNet(rate);
            }
        }


        public static DataPoint[] creatData(int amount){
            System.Random rng = new System.Random();
            DataPoint[] completeData = new DataPoint[amount];
            for(int i = 0;i<amount;i++){
                double [] inputData = new double[2];
                int randomNumberInt = rng.Next(100);
                double randomNumber = (double)randomNumberInt;
                
                
                

                if(rng.Next(2)==1){
                    
                    inputData[0] = randomNumber;
                    inputData[1] = randomNumber*randomNumber;
                    completeData[i] = new DataPoint(inputData,1,2);
                }
                else{
                    int randomNumberInt2 = rng.Next(100);
                    if (randomNumberInt2 == randomNumberInt)
                    {
                        randomNumberInt2 = rng.Next(100);
                    }
                    double randomNumber2 = (double)randomNumberInt2;
                    inputData[0] = randomNumber;
                    inputData[1] = randomNumber*randomNumber2;
                    completeData[i] = new DataPoint(inputData,0,2);
                }
                
            }
            
            
            return completeData;
        }

        public static DataPoint[] creatDataPictures(){
            
            DataPoint[] completeDataTrainng = new DataPoint[60000];
            int i = 0;
            foreach (var image in MnistReader.ReadTrainingData()){
                double [] inputDataImage = new double[image.Data.Length];
                inputDataImage = Extensions.conv2dTo1d(image.Data,784,28,28);                
                completeDataTrainng[i] = new DataPoint(inputDataImage,image.Label,10);
                i++;
            }
            
            return completeDataTrainng;
        }

        public static DataPoint[] creatDataPicturesTest(){
            //System.Random rng = new System.Random();
            DataPoint[] completeDataTrainng = new DataPoint[10000];
            int i = 0;
            foreach (var image in MnistReader.ReadTestData()){
                double [] inputDataImage = new double[image.Data.Length];
                inputDataImage = Extensions.conv2dTo1d(image.Data,784,28,28);
                completeDataTrainng[i] = new DataPoint(inputDataImage,image.Label,10);
                i++;
            }
            return completeDataTrainng;
        }


        public static DataPoint[] batchMaker(DataPoint[] fullData, int batchSize){
            DataPoint[] batchData = new DataPoint[batchSize];
            System.Random rng = new System.Random();
            for (int i = 0; i < batchSize; i++)
            {
                int rndIndex = rng.Next(fullData.Length);
                batchData[i] = fullData[rndIndex];
            }
            return batchData;
        }

    }
}