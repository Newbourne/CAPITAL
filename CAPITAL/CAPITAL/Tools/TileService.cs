using System;
using System.Linq;
using System.Windows;
using CAPITAL.Messages;
using CAPITAL.CapitalService;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;

namespace CAPITAL.Tools
{
    public class TileService
    {
        public TileService()
        {
            //Messenger.Default.Register<StatementTileMessage>(this, s => PinStatement(s.Statement));
           // Messenger.Default.Register<AccountTileMessage>(this, a => PinAccount(a.Account));
         //   Messenger.Default.Register<TileMessage>(this, tile => CreateTile(tile));
        }

        //public void PinStatement(Statement statement)
        //{
        //    string navParam = string.Format("Statement={0}", statement.StatementId);

        //    ShellTile TileToFind = ShellTile.ActiveTiles.Where(x => x.NavigationUri.ToString().Contains(navParam)).FirstOrDefault();

        //    // Create the tile if we didn't find it already exists.
        //    if (TileToFind == null)
        //    {
        //        Messenger.Default.Send(new TileMessage
        //        {
        //            Type = TileType.Statement,
        //            Id = statement.StatementId,
        //            Uri = Shared.TileService.CreateStatementTile(statement),
        //            IsNew = true
        //        });
        //    }
        //    else
        //    {
        //        //// Update Old Statement Image
        //        //StatementTile frontImage;
        //        //frontImage = new StatementFrontTile();
        //        //frontImage.Account = statement.Account;
        //        //frontImage.Amount = statement.Source.Balance;
        //        //frontImage.Date = statement.Source.DueDate;
        //        //frontImage.Id = statement.Source.StatementId;
        //        //placeholder.Children.Add(frontImage);
        //        //placeholder.UpdateLayout();
        //        //Messenger.Default.Send(new TileMessage { Type = TileType.Statement, Id = statement.Source.StatementId, Uri = frontImage.ToUri(), IsNew = false });
        //    }
        //}

        //public void PinAccount(Account Account)
        //{

        //}

        //public void CreateTile(TileMessage msg)
        //{
        //    StandardTileData NewTileData = new StandardTileData
        //    {
        //        BackgroundImage = msg.Uri,
        //        BackBackgroundImage = new Uri("/Images/Capital.png", UriKind.RelativeOrAbsolute)
        //    };

        //    Uri uri = new Uri(string.Format("/Views/MainView.xaml?{0}={1}", msg.Type, msg.Id), UriKind.Relative);

        //    if (msg.IsNew)
        //    {
        //        // Create the tile and pin it to Start. This will cause a navigation to Start and a deactivation of our application.
        //        ShellTile.Create(uri, NewTileData);
        //    }
        //    else
        //    {
        //        var tile = ShellTile.ActiveTiles.Where(x => x.NavigationUri == uri).FirstOrDefault();
        //        if (tile != null)
        //        {
        //            Deployment.Current.Dispatcher.BeginInvoke(() =>
        //            {
        //                tile.Update(NewTileData);
        //            });
        //        }
        //    }
        //}
    }
}
