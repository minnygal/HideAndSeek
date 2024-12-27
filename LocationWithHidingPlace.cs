using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class LocationWithHidingPlace : Location
    {
        /// <summary>
        /// Name of hiding place
        /// </summary>
        public string HidingPlace { get; private set; }

        /// <summary>
        /// Opponents hidden in this hiding place
        /// </summary>
        private List<Opponent> opponentsHiding = new List<Opponent>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roomName">Name of room hiding place is in</param>
        /// <param name="hidingPlaceDescription">Name of hiding place</param>
        public LocationWithHidingPlace(string roomName, string hidingPlaceDescription) : base(roomName)
        {
            HidingPlace = hidingPlaceDescription;
        }

        /// <summary>
        /// Hide opponent in hiding place
        /// </summary>
        /// <param name="opponent">Opponent to hide</param>
        public void Hide(Opponent opponent)
        {
            opponentsHiding.Add(opponent);
        }

        /// <summary>
        /// Check hiding place for opponents (clears hiding place of all opponents)
        /// </summary>
        /// <returns>List of opponents found in hiding place</returns>
        public List<Opponent> CheckHidingPlace()
        {
            List<Opponent> opponentsFound = new List<Opponent>(opponentsHiding); // Store opponents found in new list
            opponentsHiding.Clear(); // Clear list of opponents hiding
            return opponentsFound; // Return list of opponents found
        }
    }
}