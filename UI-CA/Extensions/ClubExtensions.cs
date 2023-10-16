/***************************************
 *                                     *
 *   Created by Elias De Hondt         *
 *   Visit https://eliasdh.com         *
 *                                     *
 ***************************************/
// Class ClubExtensions
using PadelClubManagement.BL.Domain;

namespace PadelClubManagement.UI.CA.Extensions;

public static class ClubExtensions
{
    public static string GetInfoBrief(this Club club) // Override ToString() method
    {
        return $"{club.Name} ({club.ClubNumber}) has {club.NumberOfCours} courts and is located at {club.StreetName} {club.HouseNumber}, {club.ZipCode}.";
    }
}