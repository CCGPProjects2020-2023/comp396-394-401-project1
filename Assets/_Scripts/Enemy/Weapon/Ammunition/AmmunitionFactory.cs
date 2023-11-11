/*
 * 
    Author's Name:          Audrey Bernier Larose based on this tutorial: https://www.patrykgalach.com/2019/03/28/implementing-factory-design-pattern-in-unity/
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 8, 2023
    Program Description:    Generic factory subclass used to specify the type of object to instantiate.
    Revision History:       October 28, 2023: Initial script and documentation.
                            November 8, 2023: Changed the type from Bullet to Ammunition
 */

public class AmmunitionFactory : Factory<Ammunition> { }
