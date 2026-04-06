    using UnityEngine;
    using TMPro;

    public class CurrencyUI : MonoBehaviour
    {
        // Référence au composant TextMeshPro pour afficher la monnaie "soft" (ex : pièces)
        [SerializeField] private TextMeshProUGUI _softText;
        // Référence au composant TextMeshPro pour afficher la monnaie "hard" (ex : gemmes)
        [SerializeField] private TextMeshProUGUI _hardText;

        private void Start()
        {
            // S'abonner aux événements du CurrencyManager pour mettre à jour l'UI quand les valeurs changent.
            CurrencyManager.Instance.OnSoftCurrencyChanged.AddListener(UpdateSoftUI);
            CurrencyManager.Instance.OnHardCurrencyChanged.AddListener(UpdateHardUI);

            // Initialiser l'affichage avec les valeurs courantes du gestionnaire de monnaie.
            UpdateSoftUI(CurrencyManager.Instance.SoftCurrency);
            UpdateHardUI(CurrencyManager.Instance.HardCurrency);
        }

        // Met à jour le texte de la monnaie "soft" avec le montant fourni.
        private void UpdateSoftUI(int amount) => _softText.text = amount.ToString();

        // Met à jour le texte de la monnaie "hard" avec le montant fourni.
        private void UpdateHardUI(int amount) => _hardText.text = amount.ToString();

        private void OnDestroy()
        {
            // Se désabonner des événements pour éviter les références persistantes après destruction.
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.OnSoftCurrencyChanged.RemoveListener(UpdateSoftUI);
                CurrencyManager.Instance.OnHardCurrencyChanged.RemoveListener(UpdateHardUI);
            }
        }
    }