using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiEngine.Elements.Particle
{
    public class modifierInterpolator
    {
        #region Vars
        private float _valueInit = 0;
        private float _valueMiddle = 0.3f;
        private float _valueFinal = 0;

        private float _middlePosition = 0.5f;
        #endregion

        #region Init
        public modifierInterpolator()
        { 
        }
        #endregion

        #region Member
        public float GetValue(float age)
        {
            if (age < _middlePosition)
            {
                return _valueInit + ((_valueMiddle - _valueInit) * (age / _middlePosition));
            }
            else
            {
                return _valueMiddle + ((_valueFinal - _valueMiddle) * ((age - _middlePosition) / (1f - _middlePosition)));
            }
        }
        #endregion

        #region Properties
        public float ValueInit
        {
            get { return _valueInit; }
            set { _valueInit = value; }
        }

        public float ValueMiddle
        {
            get { return _valueMiddle; }
            set { _valueMiddle = value; }
        }

        public float ValueFinal
        {
            get { return _valueFinal; }
            set { _valueFinal = value; }
        }

        public float MiddlePosition
        {
            get { return _middlePosition; }
            set { _middlePosition = value; }
        }
        #endregion
    }
}
