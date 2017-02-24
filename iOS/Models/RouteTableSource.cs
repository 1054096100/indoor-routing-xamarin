﻿// <copyright file="RouteTableSource.cs" company="Esri, Inc">
//      Copyright 2017 Esri.
//
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.
// </copyright>
// <author>Mara Stoica</author>
namespace IndoorRouting.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Esri.ArcGISRuntime.Data;
    using Foundation;
    using UIKit;

    /// <summary>
    /// Route table source.
    /// </summary>
    public class RouteTableSource : UITableViewSource
    {
        /// <summary>
        /// The items in the table.
        /// </summary>
        private readonly IEnumerable<Feature> items;

        /// <summary>
        /// The cell identifier for the start cell.
        /// </summary>
        private readonly string startCellIdentifier;

        /// <summary>
        /// The end cell identifier for the end cell.
        /// </summary>
        private readonly string endCellIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:IndoorRouting.iOS.RouteTableSource"/> class.
        /// </summary>
        /// <param name="items">Table Items.</param>
        internal RouteTableSource(List<Feature> items)
        {
            if (items != null)
            {
                this.items = items;
                this.startCellIdentifier = "startCellID";
                this.endCellIdentifier = "endCellID";
            }
        }

        /// <summary>
        /// Called by the TableView to determine how many cells to create for that particular section.
        /// </summary>
        /// <returns>The rows in section.</returns>
        /// <param name="tableview">Containing Tableview.</param>
        /// <param name="section">Specific Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            try
            {
                return this.items.Count();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Called by the TableView to get the actual UITableViewCell to render for the particular row
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell;
            if (indexPath.Row % 2 == 1)
            {
                cell = tableView.DequeueReusableCell(this.endCellIdentifier);
            }
            else
            {
                cell = tableView.DequeueReusableCell(this.startCellIdentifier);
            }

            try
            {
                if (this.items.ElementAt(indexPath.Row) != null)
                {
                    var item = this.items.ElementAt(indexPath.Row);
                    cell.TextLabel.Text = item.Attributes[AppSettings.CurrentSettings.LocatorFields[0]].ToString();
                    cell.DetailTextLabel.Text = string.Format("Floor {0}", item.Attributes[AppSettings.CurrentSettings.RoomsLayerFloorColumnName]);

                    return cell;
                }
                else if (AppSettings.CurrentSettings.IsLocationServicesEnabled)
                {
                    cell.TextLabel.Text = "Current Location";
                    return cell;
                }
                else
                {
                    cell.TextLabel.Text = "Unknown Location";
                    return cell;
                }
            }
            catch
            {
                cell.TextLabel.Text = "Unknown Location";
                return cell;
            }
        }
    } 
}