using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Linq;

namespace LajtIt.Dal
{
    public partial class ProductFileImportHelper
    {

        public List<ProductCatalogFileView> GetFiles()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogFileView.OrderByDescending(x => x.InsertDate).ToList();
            }
        }

        public List<ProductCatalogFileSpecificationView> GetProductCatalogFileSpecification()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogFileSpecificationView.ToList();
            }
        }

        //public void SetFile(ProductCatalogFile file, )
        //{
        //    using (LajtitDB ctx = new LajtitDB())
        //    {
        //        ctx.ProductCatalogFile.InsertOnSubmit(file);
        //        ctx.SubmitChanges();
        //    }
        //}

        public void SetFile(List<ProductCatalogFileData> products)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.ProductCatalogFileData.InsertAllOnSubmit(products);
                ctx.SubmitChanges();
            }
        }

        public List<Dal.ProductCatalogFileDataFnResult> GetProductFileData(int fileId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogFileDataFn(fileId ).ToList();
            }
        }

        public ProductCatalogFile GetFileToImport(Dal.Helper.FileImportStatus status)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogFile.Where(x => x.FileImportStatusId == (int)status).FirstOrDefault();

            }
        }

        public List<ProductCatalogAttributeGroupDict> GetProductCatalogAttributeGroupDict()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.ProductCatalogAttributeGroupDict.ToList();

            }
        }

        public List<ProductCatalogFileValidation> GetFileValidationResults(int fileId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogFileValidation.Where(x => x.ProductCatalogFileData.ProductCatalogFileId == fileId).ToList();
            }
        }

        public Dal.ProductCatalogFile GetFile(int fileId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.ProductCatalogFile.Where(x => x.ProductCatalogFileId == fileId).FirstOrDefault();
            }
        }

        public void SetFileUpdate(ProductCatalogFile file)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogFile fileToUpdate = ctx.ProductCatalogFile.Where(x => x.ProductCatalogFileId == file.ProductCatalogFileId).FirstOrDefault();

                fileToUpdate.FileImportStatusId = file.FileImportStatusId;
                fileToUpdate.ImportActionTypeId = file.ImportActionTypeId;
                fileToUpdate.ImportUpdateFields = file.ImportUpdateFields;
                fileToUpdate.CheckDuplicates = file.CheckDuplicates;
                fileToUpdate.JoinByColumn = file.JoinByColumn;


                ctx.SubmitChanges();
            }
        }

        public List<ProductCatalogFileSpecification> GetFileSpecification(int fileTypeId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ProductCatalogFileSpecification>(x => x.ProductCatalogAttributeGroup);
                dlo.LoadWith<ProductCatalogFileSpecification>(x => x.ProductCatalogAttribute);

                ctx.LoadOptions = dlo;
                return ctx.ProductCatalogFileSpecification.Where(x => x.FileTypeId == fileTypeId).ToList();
            }
        }

        public void SetFileUpdateStatus(ProductCatalogFile file)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ProductCatalogFile fileToUpdate = ctx.ProductCatalogFile.Where(x => x.ProductCatalogFileId == file.ProductCatalogFileId).FirstOrDefault();
                
                fileToUpdate.FileImportStatusId = file.FileImportStatusId;

                ctx.SubmitChanges();
            }
        }
        public void SetFileDataUpdate(LajtitDB ctx, int fileDataId, Dal.Helper.FileImportStatus status, string comment)
        { 
                ProductCatalogFileData fileToUpdate = ctx.ProductCatalogFileData.Where(x => x.FileDataId == fileDataId).FirstOrDefault();

                fileToUpdate.Comment = comment;
                fileToUpdate.FileImportStatusId = (int)status;

                ctx.SubmitChanges();
           
        }


        public void SetFileDataUpdate(int fileDataId, Dal.Helper.FileImportStatus status, string comment)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                SetFileDataUpdate(ctx, fileDataId, status, comment);
            }
        }

        public void SetFileValidationResult(int fileDataId, List<FileImportValidator> errors)
        {
            using (LajtitDB ctx = new LajtitDB())
            { 

                List<ProductCatalogFileValidation> validationToDelete = ctx.ProductCatalogFileValidation.Where(x => x.FileDataId == fileDataId).ToList();

                ctx.ProductCatalogFileValidation.DeleteAllOnSubmit(validationToDelete);

                List<ProductCatalogFileValidation> val = new List<ProductCatalogFileValidation>();
                List<int> errorsCount = new List<int>();

                foreach (FileImportValidator error in errors)
                {
                    foreach (string err in error.ValidationErrors)

                    {
                        val.Add(
                            new ProductCatalogFileValidation()
                            {
                                ErrorMsg = err,
                                FieldName = error.FieldName,
                                FileDataId = error.FileDataId
                            });

                    }
                    if (!error.ValidationResult)
                        errorsCount.Add(error.ValidationErrors.Count);

                }
                int errorCounter = errorsCount.Sum(x=>x);
                if (errorCounter > 0)
                    SetFileDataUpdate(ctx, fileDataId, Dal.Helper.FileImportStatus.Error, String.Format("Błędów: {0}", errorCounter));
                else
                    SetFileDataUpdate(ctx, fileDataId, Dal.Helper.FileImportStatus.Ok, "");

                ctx.ProductCatalogFileValidation.InsertAllOnSubmit(val);

                ctx.SubmitChanges();



            }
        }
    }
}
