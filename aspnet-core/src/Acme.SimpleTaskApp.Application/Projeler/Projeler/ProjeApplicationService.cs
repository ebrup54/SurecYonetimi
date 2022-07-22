﻿using Abp.Domain.Repositories;
using Abp.UI;
using Acme.SimpleTaskApp.Projeler;
using Acme.SimpleTaskApp.Projeler.Projeler.ProjelerDtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Projeler.Projeler
{

    public class ProjeAppService : IProjeAppService
    {
        private readonly IRepository<Proje> _repository;
        public ProjeAppService(IRepository<Proje> repository)
        {
            _repository = repository;
        }




        public async Task<List<ProjeDto>> GetProjeList()
        {
            //selam 
            //var entitylist = await _repository.GetAll().Where(a => a.Musteri.FullName == "TEst")
            //    .Take(10).Skip(0).ToListAsync();
            //var toplamCount = await _repository.GetAll().Where(a => a.Musteri.FullName == "TEst").CountAsync();
           
            var entitylist = await _repository.GetAllListAsync();
            return entitylist.Select(e => new ProjeDto
            {
                ProjeId = e.Id,
                ProjeAdi = e.ProjeAdi,
                Description = e.Description,
                BaslamaTarihi = e.BaslamaTarihi,
                Durum= e.Durum,
                BitisTarihi = e.BitisTarihi
            }).ToList();
        }





        public async Task ProjeEkle(ProjeEkleDto input)
        {
            if (string.IsNullOrEmpty(input.ProjeAdi))
            {
                throw new UserFriendlyException("Proje Adı Boş Olamaz");
            }
         

            var entity = new Proje
            {
                ProjeAdi = input.ProjeAdi,
                Description = input.Description,
                MusteriId=input.MusteriId,
            };
            await _repository.InsertAsync(entity);

        }




        public async Task ProjeGuncelle(ProjeGuncelleDto input)
        {
            if (!input.ProjeId.HasValue || input.ProjeId == 0)
            {
                throw new UserFriendlyException("Geçersiz Proje Id");
            }
            if (string.IsNullOrEmpty(input.ProjeAdi))
            {
                throw new UserFriendlyException("Proje Adı Boş Olamaz");
            }
            if (!input.MusteriId.HasValue || input.MusteriId == 0)
            {
                throw new UserFriendlyException("Geçersiz Müşteri Id");
            }

            //-
            var entity = await _repository.GetAsync(input.ProjeId.Value);
            entity.ProjeAdi = input.ProjeAdi;
            entity.Description = input.Description;
            entity.BitisTarihi = input.BitisTarihi;
            entity.Durum = input.Durum;

            await _repository.UpdateAsync(entity);

        }





        public async Task DeleteProje(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
