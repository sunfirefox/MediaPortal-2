#region Copyright (C) 2007-2008 Team MediaPortal

/*
    Copyright (C) 2007-2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using MediaPortal.Presentation.Properties;
using SlimDX;
using Presentation.SkinEngine.Xaml.Interfaces;
using MediaPortal.Utilities.DeepCopy;

namespace Presentation.SkinEngine.Controls.Transforms
{
  public class TransformGroup : Transform, IAddChild<Transform>
  {
    #region Private fields

    Property _childrenProperty;

    #endregion

    #region Ctor

    public TransformGroup()
    {
      Init();
      Attach();
    }

    void Init()
    {
      _childrenProperty = new Property(typeof(TransformCollection), new TransformCollection());
    }

    void Attach()
    {
      _childrenProperty.Attach(OnPropertyChanged);
      Children.Attach(OnPropertyChanged);
    }

    void Detach()
    {
      _childrenProperty.Detach(OnPropertyChanged);
      Children.Detach(OnPropertyChanged);
    }

    public override void DeepCopy(IDeepCopyable source, ICopyManager copyManager)
    {
      Detach();
      base.DeepCopy(source, copyManager);
      TransformGroup g = source as TransformGroup;
      foreach (Transform t in g.Children)
        Children.Add(copyManager.GetCopy(t));
      Attach();
    }

    #endregion

    protected void OnPropertyChanged(Property property)
    {
      _needUpdate = true;
      _needUpdateRel = true;
      Fire();
    }

    public Property ChildrenProperty
    {
      get { return _childrenProperty; }
    }

    public TransformCollection Children
    {
      get { return (TransformCollection)_childrenProperty.GetValue(); }
    }

    public override void UpdateTransform()
    {
      base.UpdateTransform();
      _matrix = Matrix.Identity;
      foreach (Transform t in Children)
      {
        Matrix m;
        t.GetTransform(out m);
        _matrix *= m;
      }
    }

    public override void UpdateTransformRel()
    {
      base.UpdateTransformRel();
      _matrixRel = Matrix.Identity;
      foreach (Transform t in Children)
      {
        Matrix m;
        t.GetTransformRel(out m);
        _matrixRel *= m;
      }
    }

    #region IAddChild Members

    public void AddChild(Transform o)
    {
      Children.Add(o);
    }

    #endregion
  }
}
