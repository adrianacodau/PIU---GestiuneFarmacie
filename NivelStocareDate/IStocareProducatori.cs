using System.Collections.Generic;
using LibrarieModele;

namespace NivelStocareDate
{
    public interface IStocareProducatori
    {
        void AddProducator(Producator producator);
        List<Producator> GetProducatori();
        Producator? GetProducator(int id);
        List<Producator> CautaDupaNume(string nume);
        List<Producator> CautaDupaTara(TaraOrigine tara);
        bool UpdateProducator(Producator producatorActualizat);
        bool DeleteProducator(int id);
    }
}