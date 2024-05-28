using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core.Environments;
using UnityEngine.Purchasing.Security;
using Samples.Purchasing.Core.LocalReceiptValidation;

public class IAP : Singleton<IAP> , IDetailedStoreListener
{
    public readonly string productId3900 = "juwel_3900";
    public readonly string productId6900 = "juwel_6900";
    public readonly string productId10900= "juwel_10900";
    public readonly string productId29900 = "juwel_29900";
    const string k_Environment = "production";
    private IStoreController storeController; //���� ������ �����ϴ� �Լ� ������
    private IGooglePlayStoreExtensions storeExtensionProvider; //���� �÷����� ���� Ȯ�� ó�� ������
    bool m_UseAppleStoreKitTestCertificate;
    CrossPlatformValidator m_Validator = null;

    public bool IsPurchase = false;

    public override void Awake()
    {
        base.Awake();
        Initialize(OnSuccess, OnError);
       // InitUnityIAP();
    }

    static bool IsCurrentStoreSupportedByValidator()
    {
        //The CrossPlatform validator only supports the GooglePlayStore and Apple's App Stores.
        return IsGooglePlayStoreSelected() || IsAppleAppStoreSelected();
    }

    static bool IsGooglePlayStoreSelected()
    {
        var currentAppStore = StandardPurchasingModule.Instance().appStore;
        return currentAppStore == AppStore.GooglePlay;
    }

    static bool IsAppleAppStoreSelected()
    {
        var currentAppStore = StandardPurchasingModule.Instance().appStore;
        return currentAppStore == AppStore.AppleAppStore ||
            currentAppStore == AppStore.MacAppStore;
    }

    void InitializeValidator()
    {
        if (IsCurrentStoreSupportedByValidator())
        {
#if !UNITY_EDITOR
                var appleTangleData = m_UseAppleStoreKitTestCertificate ? AppleStoreKitTestTangle.Data() : AppleTangle.Data();
                m_Validator = new CrossPlatformValidator(GooglePlayTangle.Data(), appleTangleData, Application.identifier);
#endif
        }
    }


    void Initialize(Action onSuccess, Action<string> onError)
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName(k_Environment);

            UnityServices.InitializeAsync(options).ContinueWith(task => onSuccess?.Invoke());
            InitUnityIAP();
        }
        catch (Exception exception)
        {
            onError.Invoke(exception.Message);
        }
    }
    void OnSuccess()
    {
        var text = "Congratulations!\nUnity Gaming Services has been successfully initialized.";
        Debug.Log(text);

        
    }

    void OnError(string message)
    {
        var text = $"Unity Gaming Services failed to initialize with error: {message}.";
        Debug.LogError(text);
    }

    private void InitUnityIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(productId3900, ProductType.Consumable);
        builder.AddProduct(productId6900, ProductType.Consumable);
        builder.AddProduct(productId10900, ProductType.Consumable);
        builder.AddProduct(productId29900, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }



    // �ʱ�ȭ ����
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions.GetExtension<IGooglePlayStoreExtensions>();
        InitializeValidator();

        //storeExtensionProvider.RestoreTransactions((seccess , error) => 
        //{
        //    if (seccess)
        //    {
        //        Debug.Log("�ϱ� ����");
        //    }
        //});
    }

    //�ʱ�ȭ ����
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("�������� �ʱ�ȭ ����");
        GameDataBase.Instance.logManager.Show($"�������� �ʱ�ȭ ����.").Forget();

    }
    //�ʱ�ȭ ����
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"�������� �ʱ�ȭ ���� : {message}");
        GameDataBase.Instance.logManager.Show($"�������� �ʱ�ȭ ���� : {message}").Forget();
    }

    //���Ž���
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureReason)
    {
        Debug.Log("���Ž��� �� �������");
        GameDataBase.Instance.logManager.Show("���Ž��� �� �������").Forget();

    }
    //���Ž���
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("���Ž��� �� �������");
        GameDataBase.Instance.logManager.Show("���Ž��� �� �������").Forget();
    }
    //���ż���
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(args.purchasedProduct.receipt);

        var product = args.purchasedProduct;
        var isPurchaseValid = IsPurchaseValid(product);

        if (isPurchaseValid)
        { 
            if (args.purchasedProduct.definition.id == productId3900)
            {
                Debug.Log("���� 3900�� ���ſϷ�");
                GameDataBase.Instance.saveData.daia += 10;
                GameDataBase.Instance.logManager.Show($"���� {10}�� �����ϼ̽��ϴ�.").Forget();

            }
            if (args.purchasedProduct.definition.id == productId6900)
            {
                Debug.Log("���� 6900�� ���ſϷ�");
                GameDataBase.Instance.saveData.daia += 20;
                GameDataBase.Instance.logManager.Show($"���� {20}�� �����ϼ̽��ϴ�.").Forget();
            }
        }


        return PurchaseProcessingResult.Complete;
    }


    public void PrudectJuwel(string productId)
    {
        Product product = storeController.products.WithID(productId); //��ǰ ����

        if (product != null /*&& product.availableToPurchase*/) //��ǰ�� �����ϸ鼭 ���� �����ϸ�
        {
            storeController.InitiatePurchase(product); //���Ű� �����ϸ� ����
        }
        else //��ǰ�� �������� �ʰų� ���� �Ұ����ϸ�
        {
            GameDataBase.Instance.logManager.Show("��ǰ�� ���ų� ���� ���Ű� �Ұ����մϴ�.").Forget();
        }
    }


    bool IsPurchaseValid(Product product)
    {
        //If we the validator doesn't support the current store, we assume the purchase is valid
        if (IsCurrentStoreSupportedByValidator())
        {
            try
            {
                var result = m_Validator.Validate(product.receipt);

                //The validator returns parsed receipts.
                LogReceipts(result);
            }

            //If the purchase is deemed invalid, the validator throws an IAPSecurityException.
            catch (IAPSecurityException reason)
            {
                Debug.Log($"Invalid receipt: {reason}");
                return false;
            }
        }

        return true;
    }


    static void LogReceipts(IEnumerable<IPurchaseReceipt> receipts)
    {
        Debug.Log("Receipt is valid. Contents:");
        foreach (var receipt in receipts)
        {
            LogReceipt(receipt);
        }
    }

    static void LogReceipt(IPurchaseReceipt receipt)
    {
        Debug.Log($"Product ID: {receipt.productID}\n" +
            $"Purchase Date: {receipt.purchaseDate}\n" +
            $"Transaction ID: {receipt.transactionID}");

        if (receipt is GooglePlayReceipt googleReceipt)
        {
            Debug.Log($"Purchase State: {googleReceipt.purchaseState}\n" +
                $"Purchase Token: {googleReceipt.purchaseToken}");
        }

        if (receipt is AppleInAppPurchaseReceipt appleReceipt)
        {
            Debug.Log($"Original Transaction ID: {appleReceipt.originalTransactionIdentifier}\n" +
                $"Subscription Expiration Date: {appleReceipt.subscriptionExpirationDate}\n" +
                $"Cancellation Date: {appleReceipt.cancellationDate}\n" +
                $"Quantity: {appleReceipt.quantity}");
        }
    }

    public void Prodect3900()
    {
        PrudectJuwel(productId3900);
    }
    public void Prodect6900()
    {
        PrudectJuwel(productId6900);
    }

    public void Prodect10900()
    {
        PrudectJuwel(productId10900);
    }

    public void Prodect29900()
    {
        PrudectJuwel(productId29900);
    }

  
}
