import '../Advantages.css'
import React from 'react'
import Tracker from '../Tracker'

const BaseSpeed = 5

class Advantages extends React.Component
{
	render()
	{
		let defense = this.props.skills.athletics
					+ this.props.attributes.dexterity < this.props.attributes.wits
						? this.props.attributes.dexterity
						: this.props.attributes.wits
		let initiative = this.props.attributes.dexterity + this.props.attributes.composure
		let speed = BaseSpeed + this.props.attributes.strength + this.props.attributes.dexterity
		
		return (
			<div className="advantages">
				<div className="row">
					<label>Size:</label>
					<input type="text" value={this.props.advantages.size} onChange={(e) => this.props.changeHandler('size', e.target.value)} />
				</div>
				<div className="row left">
					<label>Speed:</label>
					<div>{speed}</div>
				</div>
				<div className="row left">
					<label>Defense:</label>
					<div>{defense}</div>
				</div>
				<div className="row">
					<label>Armor:</label>
					<input type="text" value={this.props.advantages.armor} onChange={(e) => this.props.changeHandler('armor', e.target.value)} />
				</div>
				<div className="row left">
					<label>Initiative:</label>
					<div>{initiative}</div>
				</div>
				<div className="row left">
					<label>Beats:</label>
					<Tracker keyWord="beats" className="dots beats" type={Tracker.Types.Single} max="5" value={this.props.advantages.beats} changeHandler={(value) => this.props.changeHandler('beats', value)} />
				</div>
				<div className="row">
					<label>XP:</label>
					<input type="text" value={this.props.advantages.experience} onChange={(e) => this.props.changeHandler('experience', e.target.value)} />
				</div>
				<div className="row left">
					<label>Arcane Beats:</label>
					<Tracker keyWord="arcaneBeats" className="dots arcaneBeats" type={Tracker.Types.Single} max="5" value={this.props.advantages.arcaneBeats} changeHandler={(value) => this.props.changeHandler('arcaneBeats', value)} />
				</div>
				<div className="row">
					<label>Arcane XP:</label>
					<input type="text" value={this.props.advantages.arcaneExperience} onChange={(e) => this.props.changeHandler('arcaneExperience', e.target.value)} />
				</div>
			</div>
		)
	}
}

export default Advantages