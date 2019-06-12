using DocumentFlow.Models;
using DocumentFlow.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class DocPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private User CurrentUser { get; set; }
        private Employee CurrentEmployee { get; set; }
        private bool IsNew { get; set; }
        private User CreatedBy { get; set; }
        private Employee CreatedByEmp { get; set; }
        private Document CurrentDocument { get; set; }

        private DateTime docData;
        public DateTime DocData { get => docData; set => Set(ref docData, value); }

        private Document currentDoc;
        public Document CurrentDoc { get => currentDoc; set => Set(ref currentDoc, value); }

        private string createdByName;
        public string CreatedByName { get => createdByName; set => Set(ref createdByName, value); }

        private string docCompany;
        public string DocCompany { get => docCompany; set => Set(ref docCompany, value); }

        private ObservableCollection<DocumentType> docTypeCollection;
        public ObservableCollection<DocumentType> DocTypeCollection { get => docTypeCollection; set => Set(ref docTypeCollection, value); }

        private DocumentType docType;
        public DocumentType DocType { get => docType; set => Set(ref docType, value); }

        private ObservableCollection<DocumentState> docStatusCollection;
        public ObservableCollection<DocumentState> DocStatusCollection { get => docStatusCollection; set => Set(ref docStatusCollection, value); }

        private DocumentState docStatus;
        public DocumentState DocStatus { get => docStatus; set => Set(ref docStatus, value); }

        private DateTime docDocumentDate;
        public DateTime DocDocumentDate { get => docDocumentDate; set => Set(ref docDocumentDate, value); }

        private Organization org;
        public Organization Org { get => org; set => Set(ref org, value); }

        private ObservableCollection<Organization> orgCollection;
        public ObservableCollection<Organization> OrgCollection { get => orgCollection; set => Set(ref orgCollection, value); }

        private ExternalContact contact;
        public ExternalContact Contact { get => contact; set => Set(ref contact, value); }

        private ObservableCollection<ExternalContact> contactCollection;
        public ObservableCollection<ExternalContact> ContactCollection { get => contactCollection; set => Set(ref contactCollection, value); }

        private ObservableCollection<Currency> currCollection;
        public ObservableCollection<Currency> CurrCollection { get => currCollection; set => Set(ref currCollection, value); }

        private Currency currency;
        public Currency Currency { get => currency; set => Set(ref currency, value); }

        private string fileName;
        public string FileName { get => fileName; set => Set(ref fileName, value); }

        private string addNewFilePath;
        public string AddNewFilePath { get => addNewFilePath; set => Set(ref addNewFilePath, value); }

        private string fileComment;
        public string FileComment { get => fileComment; set => Set(ref fileComment, value); }

        private string docComment;
        public string DocComment { get => docComment; set => Set(ref docComment, value); }

        private decimal docSum;
        public decimal DocSum { get => docSum; set => Set(ref docSum, value); }

        private string docInfoComment;
        public string DocInfoComment { get => docInfoComment; set => Set(ref docInfoComment, value); }

        private string addNewFileContent;
        public string AddNewFileContent { get => addNewFileContent; set => Set(ref addNewFileContent, value); }

        private string addFileContent;
        public string AddFileContent { get => addFileContent; set => Set(ref addFileContent, value); }

        private bool RoEditFile;
        public bool roEditFile { get => RoEditFile; set => Set(ref RoEditFile, value); }

        public DocPageViewModel(INavigationService navigationService,
                                   IMessageService messageService,
                                      AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitUser);
            Messenger.Default.Register<NotificationMessage<Document>>(this, OnHitIt);
        }

        private void OnHitUser(NotificationMessage<User> usr)
        {
            if (usr.Notification == "SendCurrentUser")
            {
                CurrentUser = usr.Content;
                CurrentEmployee = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
            }
        }

        private void OnHitIt(NotificationMessage<Document> doc)
        {
            if (doc.Notification == "NewDocument")
            {
                IsNew = true;
                CurrentDoc = doc.Content;
                DocData = DateTime.Now;
                CreatedBy = CurrentUser;
                CreatedByEmp = db.Employees.Where(e => e.UserId == CreatedBy.Id).Single();
                CreatedByName = CreatedByEmp.Name + " " + CreatedByEmp.Surname;
                DocCompany = CreatedByEmp.Company.CompanyName;
                DocTypeCollection = new ObservableCollection<DocumentType>(db.DocumentTypes);
                DocStatusCollection = new ObservableCollection<DocumentState>(db.DocumentStates);
                OrgCollection = new ObservableCollection<Organization>(db.Organizations);
                ContactCollection = new ObservableCollection<ExternalContact>(db.ExternalContacts);
                DocStatus = DocStatusCollection.Where(s => s.DocStateName == "New").Single();
                DocStatus.IsSelectable = true;
                CurrCollection = new ObservableCollection<Currency>(db.Currencies);
                AddNewFileContent = "Add new file";
                AddFileContent = "Add";
                roEditFile = false;
            }
        }


        private RelayCommand selectionChangedCommand;
        public RelayCommand SelectionChangedCommand => selectionChangedCommand ?? (selectionChangedCommand = new RelayCommand(
                () =>
                {
                    if (Org != null)
                        ContactCollection = new ObservableCollection<ExternalContact>(db.ExternalContacts.Where(c => c.Organization.Id == Org.Id)); 
                }
            ));

        private RelayCommand<ExternalContact> contact_SelectionChangedCommand;
        public RelayCommand<ExternalContact> Contact_SelectionChangedCommand => contact_SelectionChangedCommand ?? (contact_SelectionChangedCommand = new RelayCommand<ExternalContact>(
               param =>
                {
                    if (param != null)
                        Org = param.Organization;
                }
            ));

        private RelayCommand addNewFileCommand;
        public RelayCommand AddNewFileCommand => addNewFileCommand ?? (addNewFileCommand = new RelayCommand(
                async() =>
                {
                    if (CurrentDocument == null)
                    {
                        var res = messageService.ShowYesNo("Save this document?");
                        if (res)
                        {
                            CurrentDocument = new Document
                            {
                                DocDate = DocData,
                                CreatedBy = CreatedBy,
                                Company = CreatedByEmp.Company,
                                Comment = DocComment,
                                DocumentType = DocType,
                                DocumentState = DocStatus,
                                DocInfoDate = DocDocumentDate,
                                Organization = Org,
                                InfoContact = Contact,
                                DocSum = DocSum,
                                DocCurrency = Currency,
                                DocInfoComment = DocInfoComment
                            };

                            CurrentDocument.myFiles = new List<MyFile>();

                            db.Documents.Add(CurrentDocument);
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(AddNewFilePath))
                        {
                            var newFile = new MyFile
                            {
                                FileName = FileName,
                                FileUri = AddNewFilePath,
                                Doc = CurrentDocument,
                                FileComment = FileComment
                            };

                            CurrentDocument.myFiles.Add(newFile);
                        }
                        else
                        {
                            messageService.ShowError("You should select a file and fill FileName field");
                        }
                    }
                }
            ));

        private RelayCommand<MyFile> editFileCommand;
        public RelayCommand<MyFile> EditFileCommand => editFileCommand ?? (editFileCommand = new RelayCommand<MyFile>(
               param =>
               {
                   AddNewFileContent = "Edit file comment";
                   AddFileContent = "Save";
                   roEditFile = true;
               }
            ));
    }
}
