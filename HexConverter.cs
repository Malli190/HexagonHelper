using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexConverter : MonoBehaviour
{
    public static float default_size = 1f;
    /// <summary>
    /// Функция возвращает вектор точки построения гексагона
    /// Возвращает точку шестиугольника
    /// </summary>
    /// <param name="center">Центр построения Vector3 плоскость у не учитывается</param>
    /// <param name="size">Размер гексагона в пикселях</param>
    /// <param name="i">Порядковый номер угла</param>
    /// <returns></returns>
    public static Vector3 flat_hex_corner(Vector3 center, float size, int i) 
    {
        var angle_deg = 60 * i;
        var angle_rad = Mathf.PI / 180 * angle_deg;

        Vector3 result = new Vector3(center.x + size * Mathf.Cos(angle_rad), center.y, center.z + size * Mathf.Sin(angle_rad));
        return result;
    }
    public static GameHex cube_to_axial(GameCube cube) // Перевод куб в гекс
    {
        var q = cube.x;
        var r = cube.z;
        return new GameHex(q, r);
    }
    public static GameCube axial_to_cube(GameHex hex) // Перевод гекс в куб
    {
        var x = hex.q;
        var z = hex.r;
        var y = -x - z;
        return new GameCube(x, y, z);
    }
    /// <summary>
    /// преобразование гекс в пиксели
    /// </summary>
    /// <param name="hex">Входной гекс</param>
    /// <param name="size">Указывается размер ячейки</param>
    /// <returns></returns>
    public static Vector2 flat_hex_to_pixel(GameHex hex, float size)
    {
        var x = size * (3f / 2f * hex.q);
        var y = size * (Mathf.Sqrt(3) / 2 * hex.q + Mathf.Sqrt(3) * hex.r);
        //Debug.Log(x);
        return new Vector2(x, y);
    }
    // --------------------------------------------
    /// <summary>
    /// Преобразование пиксель в гекс
    /// </summary>
    /// <param name="point">Координата х у</param>
    /// <param name="size">Указывается размер ячейки</param>
    /// <returns></returns>
    public static GameHex pixel_to_flat_hex(Vector2 point, float size)
    {
        var q = (2f / 3) * point.x / size;
        var r = ((-1f / 3) * point.x + (Mathf.Sqrt(3) / 3) * point.y) / size;
        //Debug.Log(q);
        return hex_round(new GameHex(q, r));
    }
    // --------------------------------------------
    public static GameHex hex_round(GameHex hex)
    {
        return cube_to_axial(cube_round(axial_to_cube(hex)));
    }
    public static GameCube cube_round(GameCube cube)
    {
        var rx = Mathf.Round(cube.x);
        var ry = Mathf.Round(cube.y);
        var rz = Mathf.Round(cube.z);

        var x_diff = Mathf.Abs(rx - cube.x);
        var y_diff = Mathf.Abs(ry - cube.y);
        var z_diff = Mathf.Abs(rz - cube.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new GameCube(rx, ry, rz);
    }
    public static GameCube cube_direction(GameCube startCube, int direction, int index)
    {
        var cube_directions = new GameCube[]
        {
            new GameCube(startCube.x + direction, startCube.y - direction, startCube.z),
            new GameCube(startCube.x + direction, startCube.y, startCube.z - direction),
            new GameCube(startCube.x, startCube.y + direction, startCube.z - direction),
            new GameCube(startCube.x - direction, startCube.y + direction, startCube.z),
            new GameCube(startCube.x - direction, startCube.y, startCube.z + direction),
            new GameCube(startCube.x, startCube.y - direction, startCube.z + direction),
        };
        return cube_directions[index];
    }
}
public class GameHex // Класс гексагон
{
    public float q;
    public float r;
    public GameHex(float q = 0, float r = 0)
    {
        this.q = q;
        this.r = r;
    }
}
public class GameCube // Класс гексагон куб
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public GameCube(float x = 0, float y = 0, float z = 0)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
