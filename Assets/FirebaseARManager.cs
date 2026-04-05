using UnityEngine;
using TMPro; // Needed for Text (TMP)
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseARManager : MonoBehaviour
{
    [Header("Drag your Text (TMP) objects here")]
    public TMP_Text fungsi;
    public TMP_Text jumlahPin;
    public TMP_Text nama;
    public TMP_Text ukuran;

    private DatabaseReference dbReference;

    void Start()
    {
        // 1. Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                // 2. Point EXACTLY to the "Komponen" folder in your database
                dbReference = FirebaseDatabase.GetInstance("https://esp8266tim9-default-rtdb.firebaseio.com/").GetReference("Komponen");
                
                // 3. Start listening for data
                StartListeningToDatabase();
                Debug.Log("Firebase successfully connected to AR!");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    void StartListeningToDatabase()
    {
        dbReference.ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("Database Error: " + args.DatabaseError.Message);
            return;
        }

        // args.Snapshot is now looking directly at the "Komponen" folder.
        DataSnapshot snapshot = args.Snapshot;

        if (snapshot.Child("nama").Exists)
        {
            nama.text = "Nama: " + snapshot.Child("nama").Value.ToString();
        }

        if (snapshot.Child("fungsi").Exists)
        {
            fungsi.text = "Fungsi: " + snapshot.Child("fungsi").Value.ToString();
        }

        // Note: 'jumlahpin' is all lowercase now to match your new database
        if (snapshot.Child("jumlahpin").Exists)
        {
            jumlahPin.text = "Jumlah Pin: " + snapshot.Child("jumlahpin").Value.ToString();
        }

        if (snapshot.Child("ukuran").Exists)
        {
            ukuran.text = "Ukuran: " + snapshot.Child("ukuran").Value.ToString();
        }
    }
}