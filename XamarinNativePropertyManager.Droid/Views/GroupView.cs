/*
 *  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 *  See LICENSE in the source repository root for complete license information.
 */

using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using XamarinNativePropertyManager.Droid.Adapters;
using XamarinNativePropertyManager.Droid.Fragments;
using XamarinNativePropertyManager.Droid.Services;
using XamarinNativePropertyManager.ViewModels;

namespace XamarinNativePropertyManager.Droid.Views
{
    [Activity(Label = "GroupView", Theme = "@style/Theme.Light.NoActionBar",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class GroupView : MvxAppCompatActivity<GroupViewModel>
    {
        private Android.Support.Design.Widget.FloatingActionButton _editDetailsActionButton;
        private Android.Support.Design.Widget.FloatingActionButton _addFileActionButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnViewModelSet()
        {
            Title = ViewModel.Group.DisplayName;
            SetContentView(Resource.Layout.GroupActivity);
            base.OnViewModelSet();

            // Get toolbar and set title.
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.Title = Title;

            // Configure tab layout.
            var viewPager = FindViewById<ViewPager>(Resource.Id.view_pager);
            viewPager.Adapter = new GroupViewFragmentsAdapter(this);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.tab_layout);
            tabLayout.SetupWithViewPager(viewPager);

            // Get the FABs and hook up the event listeners.
            _editDetailsActionButton = FindViewById<FloatingActionButton>(Resource.Id.edit_details_fab);
            _addFileActionButton = FindViewById<FloatingActionButton>(Resource.Id.add_file_fab);
            viewPager.PageSelected += OnPageSelected;
            OnPageSelected(null, new ViewPager.PageSelectedEventArgs(
                tabLayout.SelectedTabPosition));
        }

        private void OnPageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            // Check edit FAB.
            if (e.Position == 0)
            {
                _editDetailsActionButton.Show();
            }
            else
            {
                _editDetailsActionButton.Hide();
            }

            // Check add FAB.
            if (e.Position == 2)
            {
                _addFileActionButton.Show();
            }
            else
            {
                _addFileActionButton.Hide();
            }
        }

        protected override void OnResume()
        {
            ViewModel.OnResume();
            base.OnResume();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == FilePickerService.RequestCode)
            {
                FilePickerAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(
                    ContentResolver, requestCode, resultCode, data);
                return;
            }
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(
                requestCode, resultCode, data);
        }
    }
}