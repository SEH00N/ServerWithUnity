using UnityEngine;
using UnityEngine.UIElements;

public class UserPopOver
{
    private VisualElement root;

    private Label nameLabel;
    private Label emailLabel;
    private Label expLabel;

    public string UserName {
        get => nameLabel.text;
        set => nameLabel.text = value;
    }

    public string Email {
        get => emailLabel.text;
        set => emailLabel.text = value;
    }

    private int exp;
    public int Exp {
        get => exp;
        set {
            exp = value;
            expLabel.text = exp.ToString();
        }
    }

    public UserPopOver(VisualElement root)
    {
        this.root = root;

        nameLabel = this.root.Q<Label>("NameLabel");
        emailLabel = this.root.Q<Label>("EmailLabel");
        expLabel = this.root.Q<Label>("EXPLabel");
    }

    public void PopOver(Vector2 pos)
    {
        root.style.top = new Length(pos.y, LengthUnit.Pixel);
        root.style.left = new Length(pos.x, LengthUnit.Pixel);

        root.transform.scale = new Vector3(1, 1, 1);
    }

    public void Hide()
    {
        root.transform.scale = new Vector3(1, 0, 1);
    }
}
