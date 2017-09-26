using System;
using System.Collections.Generic;
using System.Linq;
using Frontend1;
using System.Collections;

namespace seng301_asgn1.src
{
    public class VendingMachine
    {
        private List<int> vm_coinTypes;
        private int vm_buttons;
        private List<string> vm_popNames;
        private List<int> vm_popCosts;
        private List<Coin>[] vm_CoinChute;
        private List<Pop>[] vm_PopChute;
        private List<int> insertedCoins = new List<int>();
        private List<Deliverable> vm_DeliverChute = new List<Deliverable>();
        private List<Coin> vm_Earned = new List<Coin>();
        private List<Coin> vm_unloadCoin = new List<Coin>();
        private List<Pop> vm_unloadPop = new List<Pop>();
        private List<Coin> vm_insertChute = new List<Coin>();

        public VendingMachine(List<int> coinKinds, int selectionButtonCount)
        {
            this.vm_coinTypes = coinKinds;
            this.vm_buttons = selectionButtonCount;
        }

        public List<int> get_vmCoins()
        {
            return vm_coinTypes;
        }

        public int get_vmButtons()
        {
            return vm_buttons;
        }

        public void pop_Names(List<string> popnames)
        {
            this.vm_popNames = popnames;
        }

        public List<string> get_vmPopNames()
        {
            return this.vm_popNames;
        }

        public void pop_Costs(List<int> popcosts)
        {
            this.vm_popCosts = popcosts;
        }

        public List<int> get_vmPopCosts()
        {
            return vm_popCosts;
        }

        public void createChutes(int numCoinTypes, int numPopTypes)
        {
            vm_CoinChute = new List<Coin>[numCoinTypes];
            vm_PopChute = new List<Pop>[numPopTypes];
        }

        public void loadCoinChute(int coinIndex, List<Coin> coinlist)
        {
            foreach (Coin coin in coinlist)
            {
                vm_CoinChute[coinIndex].Add(coin);
            }

        }

        public void nullCoinChute(int chuteIndex)
        {
            vm_CoinChute[chuteIndex] = new List<Coin>();
        }

        public void nullPopChute(int chuteIndex)
        {
            vm_PopChute[chuteIndex] = new List<Pop>();
        }

        public List<Coin>[] get_CoinChute()
        {
            return vm_CoinChute;
        }

        public void loadPopChute(int popIndex, List<Pop> poplist)
        {
            foreach (Pop pop in poplist)
            {
                vm_PopChute[popIndex].Add(pop);
            }
        }

        public List<Pop>[] get_PopChute()
        {
            return vm_PopChute;
        }

        public void insertCoinEarn()
        {
            foreach (Coin coin in vm_insertChute)
            {
                vm_Earned.Add(coin);
            }
        }

        public void clear_insertCoins()
        {
            vm_insertChute.Clear();
            insertedCoins.Clear();
        }

        public void insertChute(Coin coin)
        {
            vm_insertChute.Add(coin);
        }

        public void insertCoin(int coin)
        {
            insertedCoins.Add(coin);
        }

        public int insertCoinTotal()
        {
            int totalSum = insertedCoins.Sum();
            return totalSum;
        }

        public void deliveryChuteCoin(Coin coin)
        {
            vm_DeliverChute.Add(coin);
        }

        public void give_Change(int change)
        {
            int tempChange = change;
            List<int> tempCoinTypes = new List<int>();
            foreach (int coin in vm_coinTypes)
            {
                tempCoinTypes.Add(coin);
            }
            tempCoinTypes.Sort();
            tempCoinTypes.Reverse();

            List<int> coinIndex = new List<int>();
            for (int p = 0; p < vm_coinTypes.Count; p++)
            {
                coinIndex.Add(vm_coinTypes.IndexOf(tempCoinTypes[p]));
            }

            List<int> amtCoinChute = new List<int>();
            foreach (int index in coinIndex)
            {
                amtCoinChute.Add(vm_CoinChute[index].Count());
            }

            for (int i = 0; i < vm_CoinChute.Count(); i++)
            {
                if (amtCoinChute[i] >= 1)
                {
                    if (change == (vm_coinTypes[coinIndex[i]] * amtCoinChute[i]))
                    {
                        for (int j = 0; j < amtCoinChute[i]; j++)
                        {
                            vm_DeliverChute.Add(vm_CoinChute[coinIndex[i]][0]);
                            vm_CoinChute[coinIndex[i]].RemoveAt(0);
                        }
                        tempChange = 0;
                    }

                    else
                    {
                        for (int k = 1; k <= amtCoinChute[i]; k++)
                        {

                            tempChange = tempChange - vm_coinTypes[coinIndex[i]];

                            if (tempChange > 0)
                            {
                                vm_DeliverChute.Add(vm_CoinChute[coinIndex[i]][0]);
                                vm_CoinChute[coinIndex[i]].RemoveAt(0);
                            }

                            else if (tempChange < 0)
                            {
                                tempChange = tempChange + vm_coinTypes[coinIndex[i]];
                            }

                            else
                            {
                                vm_DeliverChute.Add(vm_CoinChute[coinIndex[i]][0]);
                                vm_CoinChute[coinIndex[i]].RemoveAt(0);
                            }
                        }
                    }
                }
            }
        }

        public void deliveryChutePop(int popType)
        {
            vm_DeliverChute.Add(vm_PopChute[popType][0]);
            vm_PopChute[popType].RemoveAt(0);
        }

        public List<Deliverable> get_deliveryChute()
        {
            return vm_DeliverChute;
        }

        public void clear_deliveryChute()
        {
            vm_DeliverChute.Clear();
        }

        public List<Coin> unloadCoinChute()
        {
            List<int> nulltest = new List<int>();
            int nullint = 0;
            foreach (List<Coin> coinlist in vm_CoinChute)
            {
                if (coinlist == null)
                {
                    nulltest.Add(0);
                }
            }

            foreach (int num in nulltest)
            {
                if (num == 0)
                {
                    nullint = nullint + 1;
                }
            }
 
            if(nullint != vm_CoinChute.Count())
            {
                List<int> amtCoinChute = new List<int>();
                int coinCounter = 0;

                foreach (List<Coin> coin in vm_CoinChute)
                {
                    amtCoinChute.Add(coin.Count());
                }

                for (int i = 0; i < vm_CoinChute.Count(); i++)
                {
                    if (amtCoinChute[coinCounter] >= 1)
                    {
                        for (int j = 0; j < amtCoinChute[coinCounter]; j++)
                        {
                            vm_unloadCoin.Add(vm_CoinChute[coinCounter][0]);
                            vm_CoinChute[coinCounter].RemoveAt(0);
                        }
                    }
                    coinCounter++;
                }
            }
            
            return vm_unloadCoin;
        }

        public List<Coin> get_EarnedChute()
        {
            return vm_Earned;
        }

        public List<Pop> unloadPopChute()
        {
            List<int> amtPopChute = new List<int>();
            int popCounter = 0;

            foreach (List<Pop> pop in vm_PopChute)
            {
                amtPopChute.Add(pop.Count());
            }

            for (int i = 0; i < vm_PopChute.Count(); i++)
            {
                if (amtPopChute[popCounter] >= 1)
                {
                    for (int j = 0; j < amtPopChute[popCounter]; j++)
                    {
                        vm_unloadPop.Add(vm_PopChute[popCounter][0]);
                        vm_PopChute[popCounter].RemoveAt(0);
                    }

                }
                popCounter++;
            }
            return vm_unloadPop;
        }

        public List<IList> unloadVM()
        {

            List<IList> vm_unload = new List<IList> { unloadRemainingCoin(), unloadEarnedCoin(), unloadPop() };
            return vm_unload;
        }

        public List<Coin> unloadRemainingCoin()
        {
            List<Coin> list = new List<Coin>();
            foreach (Coin coin in unloadCoinChute())
            {
                list.Add(coin);
            }
            vm_unloadCoin.Clear();
            return list;
        }

        public List<Coin> unloadEarnedCoin()
        {
            List<Coin> list = new List<Coin>();
            foreach (Coin coin in vm_Earned)
            {
                list.Add(coin);
            }
            vm_Earned.Clear();
            return list;
        }

        public List<Pop> unloadPop()
        {
            List<Pop> list = new List<Pop>();
            foreach (Pop pop in unloadPopChute())
            {
                list.Add(pop);
            }
            vm_unloadPop.Clear();
            return list;
        }
    }
}
