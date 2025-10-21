using Course.Bll.Service.GenralIService;
using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;
using Mapster;

namespace Course.Bll.Service.Class
{
    public class CourseMaterialService
        : GeneralService<CourseMaterialReq, CourseMaterialRes, CourseMaterial>, ICourseMaterialService
    {
        private readonly ICourseMaterialRepo materialRepo;
        private readonly IFileService fileService;


        public CourseMaterialService (
            IGenralRepositry<CourseMaterial> repository,
            ICourseMaterialRepo materialRepo,
            IFileService fileService
        ) : base(repository)
        {
            this.materialRepo=materialRepo;
            this.fileService=fileService;
        }


        public async Task AddCourseMaterialAsync (CourseMaterialReq req, int courseId)
        {
            var material = req.Adapt<CourseMaterial>();
            material.CourseId=courseId;
            if (req.MaterialUrl!=null)
            {
                var uploadedFileName = await fileService.UploadFileAsync(req.MaterialUrl, "CourseMaterials");
                material.MaterialUrl=uploadedFileName;
            }
            await materialRepo.AddAsync(material);
        }



        public async Task<bool> DeleteAsync (int id)
        {
            var material = await materialRepo.GetByIdAsync(id);
            if (material==null)
                return false;
            if (material.MaterialUrl is not null)
            {
                await fileService.DeleteFileAsync(material.MaterialUrl, "CourseMaterials");
            }
            var deleted = await materialRepo.DeleteAsync(material);
            return deleted>0;
        }

        public async Task<bool> IsInstrctorCanAddMatirial (int courseId, string instructorId)
        {
            var result = await materialRepo.GetAllAsync(e => e.CourseId==courseId&&e.Course.InstructorId==instructorId);
            return result is not null;
        }

        public async Task<bool> UpdateMaterialAsync (int id, CourseMaterialReq req)
        {
            var existingMaterial = await materialRepo.GetByIdAsync(id);
            if (existingMaterial==null)
                return false;
            string newFile = existingMaterial.MaterialUrl;
            if (req.MaterialUrl!=null)
            {
                newFile=await fileService.UploadFileAsync(req.MaterialUrl, "CourseMaterials");
                if (!string.IsNullOrEmpty(existingMaterial.MaterialUrl))
                {
                    await fileService.DeleteFileAsync(existingMaterial.MaterialUrl, "CourseMaterials");
                }
            }
            var materialToUpdate = req.Adapt<CourseMaterial>();
            materialToUpdate.Id=id;
            materialToUpdate.MaterialUrl=newFile;
            var updated = await materialRepo.UpdateAsync(materialToUpdate);
            return updated>0;
        }

        Task<bool> ICourseMaterialService.DeleteAsync (int id)
        {
            throw new NotImplementedException();
        }
    }
}
