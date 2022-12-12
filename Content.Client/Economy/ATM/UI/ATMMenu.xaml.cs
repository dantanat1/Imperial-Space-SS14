using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using static Content.Shared.Economy.ATM.SharedATMComponent;

namespace Content.Client.Economy.ATM.UI
{
    [GenerateTypedNameReferences]
    public sealed partial class ATMMenu : DefaultWindow
    {
        public event Action<BaseButton.ButtonEventArgs, int>? OnWithdrawAttempt;
        public ATMMenu()
        {
            RobustXamlLoader.Load(this);
            WithdrawButton.OnButtonDown += OnWithdrawButtonDown;
        }
        public void UpdateState(ATMBoundUserInterfaceState state)
        {
            WelcomeLabel.Text = state.IdCardFullName != null
                ? Loc.GetString("atm-ui-welcome-label-w-name", ("name", state.IdCardFullName))
                : Loc.GetString("atm-ui-default-welcome-label");
            IdCardButton.Text = state.IdCardEntityName != null
                ? state.IdCardEntityName
                : Loc.GetString("atm-ui-no-card-label");
            AccountBalance.Text = state.BankAccountBalance != null
                ? Loc.GetString("atm-ui-account-balance", ("balance", state.BankAccountBalance))
                : Loc.GetString("atm-ui-account-balance", ("balance", "%ERR!"));
            ErrorLabel.Visible = state.IsCardPresent && !state.HaveAccessToBankAccount;
            WithdrawInput.Editable = ShowWithdraw.Visible = state.IsCardPresent && state.HaveAccessToBankAccount;
            WithdrawButton.Disabled = !ShowWithdraw.Visible;
        }
        private void OnWithdrawButtonDown(BaseButton.ButtonEventArgs args)
        {
            OnWithdrawAttempt?.Invoke(args, GetValueFromWithdrawInput());
            WithdrawInput.Clear();
        }
        private int GetValueFromWithdrawInput()
        {
            int value = 0;
            Int32.TryParse(WithdrawInput.Text, out value);
            return value;
        }
    }
}