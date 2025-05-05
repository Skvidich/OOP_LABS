using graphiclaEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace graphiclaEditor
{
    class PluginManager
    {
        private List<ConstructorInfo> _rectConstructors;
        private List<ConstructorInfo> _circleConstructors;
        private List<ConstructorInfo> _polyConstructors;
        private Action _refreshUi;

        public PluginManager(
            ref List<ConstructorInfo> rectConstructors,
            ref List<ConstructorInfo> circleConstructors,
            ref List<ConstructorInfo> polyConstructors,
            Action refreshUi)
        {
            _rectConstructors = rectConstructors;
            _circleConstructors = circleConstructors;
            _polyConstructors = polyConstructors;
            _refreshUi = refreshUi;
        }

        public void AddPlugin(string filePath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(filePath);
                var rectTypes = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(RectBase)) && !t.IsAbstract);
                var circleTypes = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(CircleBase)) && !t.IsAbstract);
                var polyTypes = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(PolyBase)) && !t.IsAbstract);

                LoadConstructors(rectTypes, typeof(RectBase), new[] { typeof(Cords), typeof(Cords) }, _rectConstructors);
                LoadConstructors(circleTypes, typeof(CircleBase), new[] { typeof(Cords), typeof(Cords), typeof(int) }, _circleConstructors);
                LoadConstructors(polyTypes, typeof(PolyBase), new[] { typeof(List<Cords>) }, _polyConstructors);

                _refreshUi?.Invoke();
                MessageBox.Show("Plugin loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading plugin: {ex.Message}");
            }
        }

        private void LoadConstructors(IEnumerable<Type> types, Type baseType, Type[] constrParams, List<ConstructorInfo> targetList)
        {
            foreach (var type in types)
            {
                var ctor = type.GetConstructor(constrParams);
                if (ctor != null && !targetList.Any(ci => ci.DeclaringType == type))
                {
                    targetList.Add(ctor);
                }
            }
        }
    }
}