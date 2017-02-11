using System;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;

public class VendingMachineFactory : IVendingMachineFactory {
    List<VendingMachine> vendingMachines;
    public VendingMachineFactory()
    {
        this.vendingMachines = new List<VendingMachine>();
    }
    public int CreateVendingMachine(List<int> coinKinds, int selectionButtonCount, int coinRackCapacity, int popRackCapcity, int receptacleCapacity) {
        var index = this.vendingMachines.Count;
        int[] coinKindss = coinKinds.ToArray();
        this.vendingMachines.Add(new VendingMachine(coinKindss, selectionButtonCount, coinRackCapacity, popRackCapcity, receptacleCapacity));
        return index;
    }

    public void ConfigureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {
        this.vendingMachines[vmIndex].Configure(popNames, popCosts);
    }

    public void LoadCoins(int vmIndex, int coinKindIndex, List<Coin> coins) {
        int coin = this.vendingMachines[vmIndex].GetCoinKindForCoinRack(coinKindIndex);
        this.vendingMachines[vmIndex].GetCoinRackForCoinKind(coin).LoadCoins(coins);
        //this.vendingMachines[vmIndex].GetCoinRackForCoinKind(coin).CoinAdded += new EventHandler<CoinEventArgs>(printCoinAdded);
        //this.vendingMachines[vmIndex].GetCoinRackForCoinKind(coin).CoinRackEmpty += new EventHandler<CoinEventArgs>(printCoinRackEmpty);
        //this.vendingMachines[vmIndex].GetCoinRackForCoinKind(coin).CoinRackFull += new EventHandler<CoinEventArgs>(printCoinRackFull);
        //this.vendingMachines[vmIndex].GetCoinRackForCoinKind(coin).CoinsLoaded += new EventHandler<CoinEventArgs>(printCoinAdded);
    }

    public void LoadPops(int vmIndex, int popKindIndex, List<PopCan> pops) {
        this.vendingMachines[vmIndex].PopCanRacks[popKindIndex].LoadPops(pops);
    }

    public void InsertCoin(int vmIndex, Coin coin) {
        this.vendingMachines[vmIndex].CoinSlot.AddCoin(coin);
    }

    public void PressButton(int vmIndex, int value) {
        this.vendingMachines[vmIndex].SelectionButtons[value].Press();
        int popC = this.vendingMachines[vmIndex].PopCanCosts[value];
        int coinIn = this.vendingMachines[vmIndex].CoinReceptacle.Count;
        if (popC < coinIn)
        {
            int diff = coinIn - popC;
            for(int i = 0; i < this.vendingMachines[vmIndex].CoinRacks.Length; i++)
            {
                int coinKind = this.vendingMachines[vmIndex].GetCoinKindForCoinRack(i);
                while(diff > 0)
                 {
                    try
                        {
                            CoinRack taking = this.vendingMachines[vmIndex].GetCoinRackForCoinKind(coinKind);
                            taking.ReleaseCoin();
                            diff = diff - coinKind;
                        }
                        catch(Exception)
                        {
                            break;
                        }
                }
            }
            this.vendingMachines[vmIndex].PopCanRacks[value].DispensePopCan();
        }
        else if (popC == coinIn)
        {
            this.vendingMachines[vmIndex].PopCanRacks[value].DispensePopCan();
        }
    }

    public List<IDeliverable> ExtractFromDeliveryChute(int vmIndex) {
        IDeliverable[] returned = this.vendingMachines[vmIndex].DeliveryChute.RemoveItems();
        List<IDeliverable> toUser = new List<IDeliverable>();
        for(int i = 0; i < returned.Length; i++)
        {
            toUser.Add(returned[i]);
        }
        return toUser;
    }

    public VendingMachineStoredContents UnloadVendingMachine(int vmIndex) {
        VendingMachineStoredContents allDone = new VendingMachineStoredContents();
        List<Coin> returnCoins = vendingMachines[vmIndex].StorageBin.Unload();
        for(int i = 0; i < returnCoins.Capacity; i++)
        {
            allDone.PaymentCoinsInStorageBin.Add(returnCoins[i]);
        }
        for (int i = 0; i < this.vendingMachines[vmIndex].CoinRacks.Length; i ++)
        {
            allDone.CoinsInCoinRacks.Add(this.vendingMachines[vmIndex].CoinRacks[i].Unload());
        }
        for (int i = 0; i < this.vendingMachines[vmIndex].PopCanRacks.Length; i++)
        {
            allDone.PopCansInPopCanRacks.Add(this.vendingMachines[vmIndex].PopCanRacks[i].Unload());
        }
        Console.ReadLine();
        return allDone;
    }
}