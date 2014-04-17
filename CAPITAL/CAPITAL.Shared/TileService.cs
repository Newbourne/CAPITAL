using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CAPITAL.Shared.Tiles;
using CAPITAL.Shared.CapitalService;

namespace CAPITAL.Shared
{
    public static class TileService
    {
        // Couldn't find alternative way to render the control before processing tile
        // Canvas atm must be used for the tiles to render correctly.
        static Canvas placeholder = new Canvas();

        public static Uri CreateStatementTile(Statement statement)
        {
            StatementTile frontImage;
            frontImage = new StatementTile();
            frontImage.Account = (string)statement.AccountName;
            frontImage.Amount = statement.Balance;
            frontImage.Date = statement.DueDate;
            frontImage.Id = statement.StatementId;
            placeholder.Children.Add(frontImage);
            placeholder.UpdateLayout();

            return frontImage.ToUri();
        }
    }
}
