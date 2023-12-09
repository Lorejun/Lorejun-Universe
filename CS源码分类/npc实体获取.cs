using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;

namespace Game
{
    public class ComponententityLord: Component
    {
        private EntityReference lordEntityRef; // 保存对渡渡角色的引用
        public ComponentBody componentBody;
        public ComponentBody lordBody
        {
            get { return componentBody; }
        }
        
        public Entity LordEntity
        {
            get { return componentBody.Entity; }
        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);
           
            Entity lordEntity = Entity;

            // 将渡渡角色的EntityReference转换为可以保存的形式
           componentBody=lordEntity.FindComponent<ComponentBody>();
           // componentBody = valuesDictionary.GetValue<EntityReference>("LordEntity").GetComponent<ComponentBody>(base.Entity, idToEntityMap, false);
            // 尝试从valuesDictionary中读取渡渡角色的EntityReference
           
            // 使用EntityReference获取渡渡角色的实体对象
             //lordEntity = lordEntityRef.GetEntity(null, idToEntityMap, false);

            // 根据需要处理lordEntity，例如获取组件或者设置属性等
            // ...
            
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);

            // 保存渡渡角色的EntityReference
            // 假设我们已经有了渡渡角色对应的Entity对象并赋值给变量lordEntity
            Entity lordEntity = componentBody.Entity;

            // 将渡渡角色的EntityReference转换为可以保存的形式
           lordEntityRef = EntityReference.FromId(lordEntity, entityToIdMap);

            // 将渡渡角色的EntityReference保存到valuesDictionary
            valuesDictionary.SetValue<EntityReference>("LordEntity", lordEntityRef);
        }

    }

    public class ComponententityDodo : Component
    {
        private EntityReference lordEntityRef; // 保存对渡渡角色的引用
        public ComponentBody componentBody;
        public ComponentBody lordBody
        {
            get { return componentBody; }
        }

        public Entity LordEntity
        {
            get { return componentBody.Entity; }
        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            base.Load(valuesDictionary, idToEntityMap);

            Entity lordEntity = Entity;

            // 将渡渡角色的EntityReference转换为可以保存的形式
            componentBody = lordEntity.FindComponent<ComponentBody>();
            // componentBody = valuesDictionary.GetValue<EntityReference>("LordEntity").GetComponent<ComponentBody>(base.Entity, idToEntityMap, false);
            // 尝试从valuesDictionary中读取渡渡角色的EntityReference

            // 使用EntityReference获取渡渡角色的实体对象
            //lordEntity = lordEntityRef.GetEntity(null, idToEntityMap, false);

            // 根据需要处理lordEntity，例如获取组件或者设置属性等
            // ...

        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);

            // 保存渡渡角色的EntityReference
            // 假设我们已经有了渡渡角色对应的Entity对象并赋值给变量lordEntity
            Entity lordEntity = componentBody.Entity;

            // 将渡渡角色的EntityReference转换为可以保存的形式
            lordEntityRef = EntityReference.FromId(lordEntity, entityToIdMap);

            // 将渡渡角色的EntityReference保存到valuesDictionary
            valuesDictionary.SetValue<EntityReference>("LordEntity", lordEntityRef);
        }

    }
}
