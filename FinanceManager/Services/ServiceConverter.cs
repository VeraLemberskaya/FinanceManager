using System.Collections.Generic;
using FinanceManager.Model;
using FinanceManager.ViewModel;

namespace FinanceManager.Services
{
     class ServiceConverter
    {
        public static List<TransactionViewModel> ConvertToTransactionVM(List<Transaction> collection)
        {
            if (collection != null)
            {
                List<TransactionViewModel> collectionVM = new();
                foreach (var transaction in collection)
                {
                    TransactionViewModel transactionVM = new TransactionViewModel(transaction);
                    collectionVM.Add(transactionVM);
                }
                return collectionVM;
            }
            return null;
        }
        public static List<CategoryViewModel> ConvertToCategoryVM(List<Category> collection)
        {
            if (collection != null)
            {
                List<CategoryViewModel> collectionVM = new();
                foreach (var category in collection)
                {
                    CategoryViewModel categoryVM = new CategoryViewModel(category);
                    collectionVM.Add(categoryVM);
                }
                return collectionVM;
            }
            return null;
        }
        public static List<AccountViewModel> ConvertToAccountVM(List<Account> collection)
        {
            if (collection != null)
            {
                List<AccountViewModel> collectionVM = new();
                foreach (var account in collection)
                {
                    AccountViewModel accountVM = new AccountViewModel(account);
                    collectionVM.Add(accountVM);
                }
                return collectionVM;
            }
            return null;
        }
    }
}
