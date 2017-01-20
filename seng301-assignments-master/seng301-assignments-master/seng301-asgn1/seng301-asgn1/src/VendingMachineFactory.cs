using System.Collections;
using System.Collections.Generic;

using Frontend1;
using System;

namespace seng301_asgn1 {
    /// <summary>
    /// Represents the concrete virtual vending machine factory that you will implement.
    /// This implements the IVendingMachineFactory interface, and so all the functions
    /// are already stubbed out for you.
    /// 
    /// Your task will be to replace the TODO statements with actual code.
    /// 
    /// Pay particular attention to extractFromDeliveryChute and unloadVendingMachine:
    /// 
    /// 1. These are different: extractFromDeliveryChute means that you take out the stuff
    /// that has already been dispensed by the machine (e.g. pops, money) -- sometimes
    /// nothing will be dispensed yet; unloadVendingMachine is when you (virtually) open
    /// the thing up, and extract all of the stuff -- the money we've made, the money that's
    /// left over, and the unsold pops.
    /// 
    /// 2. Their return signatures are very particular. You need to adhere to this return
    /// signature to enable good integration with the other piece of code (remember:
    /// this was written by your boss). Right now, they return "empty" things, which is
    /// something you will ultimately need to modify.
    /// 
    /// 3. Each of these return signatures returns typed collections. For a quick primer
    /// on typed collections: https://www.youtube.com/watch?v=WtpoaacjLtI -- if it does not
    /// make sense, you can look up "Generic Collection" tutorials for C#.
    /// </summary>
    /// 

    public class VendingMachine : VendingMachineFactory
    {
        public int buttons = 0;
        public Dictionary<string, int> popList = new Dictionary<string, int>();
        //              <Pop type, how many>
        public Dictionary<Pop, int> popChange = new Dictionary<Pop, int>();
        public Dictionary<int, int> coinKindList = new Dictionary<int, int>();
        //               <Coin type, how many>
        public Dictionary<Coin, int> coinProfit = new Dictionary<Coin, int>();
        //               <Coin type, how many>
        public Dictionary<Coin, int> coinChange = new Dictionary<Coin, int>();
        public Dictionary<int, string> popDispence = new Dictionary<int, string>();
        public void setButtons(int a)
        {
            buttons = a;
        }

        public int getButtons()
        {
            return this.buttons;
        }

        public int getProfit()
        {
            int profit = 0;
            foreach(var c in coinProfit)
            {
                profit += c.Key.Value * c.Value;
            }
            return profit;
        }
    }
    public class VendingMachineFactory : IVendingMachineFactory {
        public int vmCount = -1;
        public int cCounter = 0;
        List<VendingMachine> vmList = new List<VendingMachine>();
        public VendingMachineFactory() {

        }

        public int createVendingMachine(List<int> coinKinds, int selectionButtonCount) {
            VendingMachine VM = new VendingMachine();
            foreach(int c in coinKinds) {
                if (VM.coinKindList.ContainsKey(c))
                {
                    throw new Exception("This coin already exists");
                }
                if (c <= 0)
                {
                    throw new Exception("Cannot have negative or zero currency");
                }
                Coin temp = new Coin(c);
                VM.coinKindList.Add(c, 0);
                VM.coinProfit.Add(temp, 0);
                VM.coinChange.Add(temp, 0);
                cCounter++;
            }
            VM.setButtons(selectionButtonCount);
            vmList.Add(VM);
            vmCount++;
            return vmCount;
        }

        public void configureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {
            VendingMachine VM = vmList[vmIndex];
            if (VM.buttons < popNames.Count)
            {
                throw new Exception("Cannot have more pops than buttons, please try again");
            }
            if (popNames.Count != popCosts.Count)
            {
                throw new Exception("List sizes do not match, please try again");
            }
            else if(popNames.Count <= 0 || popCosts.Count <= 0)
            {
                throw new Exception("List is empty, please try again");
            }
            for(int i = 0; i < popNames.Count; i++)
            {
                VM.popList.Add(popNames[i], popCosts[i]);
                VM.popDispence.Add(i, popNames[i]); 
                Pop temp1 = new Pop(popNames[i]);
                VM.popChange.Add(temp1, 0);
            }
        }

        public void loadCoins(int vmIndex, int coinKindIndex, List<Coin> coins) {
            VendingMachine VM = vmList[vmIndex];
            try
            {
                Coin Key = coins[coinKindIndex];
                if (VM.coinChange.ContainsKey(Key))
                {
                    VM.coinChange[coins[coinKindIndex]] += 1;
                }
            }
            catch(ArgumentOutOfRangeException e)
            {
                Console.WriteLine("You have entered an invalid index and caused a " + e + ", please try again" );
            }

        }

        public void loadPops(int vmIndex, int popKindIndex, List<Pop> pops) {
            VendingMachine VM = vmList[vmIndex];
            try
            {
                Pop Key = pops[popKindIndex];
                if (VM.popChange.ContainsKey(Key))
                {
                    VM.popChange[pops[popKindIndex]] += 1;
                }
            }
            catch(ArgumentOutOfRangeException e)
            {
                Console.WriteLine("You have entered an invalid index and caused a " + e + ", please try again");
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine("You have provided a null value and causes a " + e + ", please try again");
            }
            
        }

        public void insertCoin(int vmIndex, Coin coin) {
            VendingMachine VM = vmList[vmIndex];
            foreach (var c in VM.coinProfit)
            {
                if (c.Key == coin)
                {
                    VM.coinProfit[c.Key] += 1;
                }
            }
        }

        public void pressButton(int vmIndex, int value) {
            VendingMachine VM = vmList[vmIndex];

        }

        public List<Deliverable> extractFromDeliveryChute(int vmIndex) {
            // TODO: Implement
            return new List<Deliverable>();
        }

        public List<IList> unloadVendingMachine(int vmIndex) {
            // TODO: Implement
            Console.ReadKey();
            return new List<IList>() {
                new List<Coin>(),
                new List<Coin>(),
                new List<Pop>() };
                
        }
    }
    
}