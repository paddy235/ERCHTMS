using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using ERCHTMS.Entity.SafeNote;
using ERCHTMS.IService.SafeNote;

namespace ERCHTMS.Service.SafeNote
{
    public class SafeNoteService : RepositoryFactory<SafeNoteEntity>, SafeNoteIService
    {
        public IEnumerable<SafeNoteEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public SafeNoteEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void RemoveForm(string keyValue)
        {
             this.BaseRepository().Delete(keyValue);
        }

        public void SaveForm(string keyValue, SafeNoteEntity entity)
        {
            SafeNoteEntity note = this.BaseRepository().FindEntity(keyValue);
            if (note!=null)
            {
                note.Modify(keyValue);
                note.Type = entity.Type;
                note.Value = entity.Value;
                note.Time = entity.Time;
                this.BaseRepository().Update(note);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
    }
}
