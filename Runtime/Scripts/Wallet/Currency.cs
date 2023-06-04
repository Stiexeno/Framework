using System;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Wallet
{
	[CreateAssetMenu(fileName = "Currency", menuName = "Framework/Wallet/Currency")]
	public class Currency : ScriptableObject
	{
		public string currencyName;
		public Sprite icon;
		public BigNumber initAmount;
		
		[NonSerialized] private CurrencyRef currencyRef;
		
		// Events

		public event Action<(BigNumber final, BigNumber added)> OnAmountChanged;
		
		// Currency

		public void Set(CurrencyRef currencyRef)
		{
			this.currencyRef = currencyRef;
		}

		public void Add(BigNumber amount)
		{
			currencyRef.amount += amount;
			OnAmountChanged?.Invoke((currencyRef.amount, amount));
		}

		public bool TrySpend(BigNumber amount)
		{
			if (CanAfford(amount) == false)
			{
				return false;
			}

			currencyRef.amount -= amount;

			if (currencyRef.amount < 0)
				currencyRef.amount = 0;

			OnAmountChanged?.Invoke((currencyRef.amount, amount * -1));

			return true;
		}
		
		public bool CanAfford(BigNumber num)
		{
			return currencyRef.amount - num >= 0;
		}

		public override string ToString()
		{
			return currencyRef.amount.ToString();
		}

		public static implicit operator BigNumber(Currency c) => c.currencyRef.amount;
	}   
}
