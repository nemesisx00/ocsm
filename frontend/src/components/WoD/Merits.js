import './Merits.css'
import React from 'react'
import EntryList from '../core/EntryList'
import Tracker from './Tracker'
import { normalizeClassNames } from '../core/Utilities'

const MeritKeys = Object.freeze({
	Label: 'label',
	New: 'new',
	Value: 'value'
})

class Merit extends React.Component
{
	render()
	{
		return (<div className="entry merit">
			<input type="text" value={this.props.label} onChange={(event) => this.props.changeHandler(this.props.index, MeritKeys.Label, event.target.value)} />
			<Tracker keyWord={this.props.label} className={normalizeClassNames(`dots ${this.props.label}`)} type={Tracker.Types.Circle} max={this.props.max} value={this.props.value} changeHandler={(value) => this.props.changeHandler(this.props.index, MeritKeys.Value, value)} />
		</div>)
	}
}

class Merits extends EntryList
{
	generateEntries()
	{
		let entries = []
		
		this.props.merits.forEach((merit, i) => {
			entries.push(<Merit key={`merit-${i}`} index={i} label={merit.label} value={merit.value} max={this.props.max} changeHandler={(index, key, value) => this.props.changeHandler(index, key, value)} />)
		})
		
		return entries
	}
	
	addNewEntry()
	{
		this.props.changeHandler(MeritKeys.New, MeritKeys.Label, '')
	}
}

Merits.Keys = MeritKeys

export default Merits
