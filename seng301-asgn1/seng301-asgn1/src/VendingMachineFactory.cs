using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Frontend1;
using seng301_asgn1.src;

namespace seng301_asgn1
{
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
    public class VendingMachineFactory : IVendingMachineFactory
    {

        private List<VendingMachine> vm_List = new List<VendingMachine>();

        public VendingMachineFactory()
        {

        }

        public int createVendingMachine(List<int> coinKinds, int selectionButtonCount)
        {

            bool checkNeg = coinKinds.Min() <= 0;
            if (checkNeg == true)
            {
                throw new Exception("Coin values are zero or negative!");
            }

            if (coinKinds.Count() != coinKinds.Distinct().Count())
            {
                throw new Exception("Coin values are the same!");
            }

            var vm = new VendingMachine(coinKinds, selectionButtonCount);
            vm_List.Add(vm);
            int vm_ID = vm_List.Count() - 1;

            return vm_ID;
        }

        public void configureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts)
        {
            bool checkNeg = popCosts.Min() <= 0;
            if (checkNeg == true)
            {
                throw new Exception("Cost of a pop is zero or negative!");
            }

            if (popCosts.Count() != popNames.Count())
            {
                throw new Exception("The number of pop names is different from the number of pop costs!");
            }

            if (popNames.Count() != vm_List[vmIndex].get_vmButtons())
            {
                throw new Exception("The number of pop names is not equal to the number of buttons for the vending machine!");
            }

            vm_List[vmIndex].pop_Names(popNames);
            vm_List[vmIndex].pop_Costs(popCosts);

            int numcoinTypes = vm_List[vmIndex].get_vmCoins().Count();
            int numpopTypes = popNames.Count();
            vm_List[vmIndex].createChutes(numcoinTypes, numpopTypes);

            for (int chuteIndex = 0; chuteIndex < vm_List[vmIndex].get_vmCoins().Count; chuteIndex++)
            {
                if (vm_List[vmIndex].get_CoinChute()[chuteIndex] == null)
                {
                    vm_List[vmIndex].nullCoinChute(chuteIndex);
                }
            }

            for (int chuteIndex = 0; chuteIndex < vm_List[vmIndex].get_vmButtons(); chuteIndex++)
            {
                if (vm_List[vmIndex].get_PopChute()[chuteIndex] == null)
                {
                    vm_List[vmIndex].nullPopChute(chuteIndex);
                }
            }
        }

        public void loadCoins(int vmIndex, int coinKindIndex, List<Coin> coins)
        {
            vm_List[vmIndex].loadCoinChute(coinKindIndex, coins);
        }

        public void loadPops(int vmIndex, int popKindIndex, List<Pop> pops)
        {

            vm_List[vmIndex].loadPopChute(popKindIndex, pops);
        }

        public void insertCoin(int vmIndex, Coin coin)
        {

            int acceptedCoin = 0;
            foreach (int coinType in vm_List[vmIndex].get_vmCoins())
            {
                if (coinType == coin.Value)
                {
                    acceptedCoin++;
                    vm_List[vmIndex].insertChute(coin);
                    vm_List[vmIndex].insertCoin(coin.Value);
                }
            }

            if (acceptedCoin != 1)
            {
                vm_List[vmIndex].deliveryChuteCoin(coin);
            }
        }
        public void pressButton(int vmIndex, int value)
        {
            if (value < 0)
            {
                throw new Exception("Invalid button press, button is negative!");
            }

            if (value > vm_List[vmIndex].get_vmButtons())
            {
                throw new Exception("Invalid button press, button is higher than the number of buttons on the vending machine!");
            }

            int insertedTotal = vm_List[vmIndex].insertCoinTotal();
            List<int> popCost = vm_List[vmIndex].get_vmPopCosts();
            List<Pop>[] popName = vm_List[vmIndex].get_PopChute();

            if (insertedTotal >= popCost[value])
            {
                
                int change = insertedTotal - popCost[value];
                
                if (popName[value].Count() >= 1)
                {
                    vm_List[vmIndex].insertCoinEarn();
                    vm_List[vmIndex].give_Change(change);
                    vm_List[vmIndex].deliveryChutePop(value);
                    vm_List[vmIndex].clear_insertCoins();        
                }
            }
        }
        public List<Deliverable> extractFromDeliveryChute(int vmIndex)
        {
            List<Deliverable> vm_DeliveryChute = new List<Deliverable>(vm_List[vmIndex].get_deliveryChute());
            vm_List[vmIndex].clear_deliveryChute();
            return vm_DeliveryChute;
        }
        public List<IList> unloadVendingMachine(int vmIndex)
        {
            return vm_List[vmIndex].unloadVM();
        }
    }
}